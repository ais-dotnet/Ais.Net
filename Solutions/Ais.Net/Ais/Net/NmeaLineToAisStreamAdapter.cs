// <copyright file="NmeaLineToAisStreamAdapter.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Ais.Net
{
    using System;
    using System.Buffers;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Processes NMEA message lines, and passes their payloads as complete AIS messages to an
    /// <see cref="INmeaAisMessageStreamProcessor"/>, reassembling any messages that were split
    /// across multiple NMEA lines.
    /// </summary>
    public class NmeaLineToAisStreamAdapter : INmeaLineStreamProcessor, IDisposable
    {
        private readonly INmeaAisMessageStreamProcessor messageProcessor;
        private readonly NmeaParserOptions options;
        private readonly Dictionary<int, FragmentedMessage> messageFragments = new Dictionary<int, FragmentedMessage>();
        private int messagesProcessed = 0;
        private int messagesProcessedAtLastUpdate = 0;

        /// <summary>
        /// Creates a <see cref="NmeaLineToAisStreamAdapter"/>.
        /// </summary>
        /// <param name="messageProcessor">
        /// The message process to which complete AIS messages are to be passed.
        /// </param>
        public NmeaLineToAisStreamAdapter(INmeaAisMessageStreamProcessor messageProcessor)
            : this(messageProcessor, new NmeaParserOptions())
        {
        }

        /// <summary>
        /// Creates a <see cref="NmeaLineToAisStreamAdapter"/>.
        /// </summary>
        /// <param name="messageProcessor">
        /// The message process to which complete AIS messages are to be passed.
        /// </param>
        /// <param name="options">Configures parser behaviour.</param>
        public NmeaLineToAisStreamAdapter(
            INmeaAisMessageStreamProcessor messageProcessor,
            NmeaParserOptions options)
        {
            this.messageProcessor = messageProcessor;
            this.options = options;
        }

        /// <inheritdoc/>
        public void OnCompleted()
        {
            this.FreeRentedBuffers();
            this.messageProcessor.OnCompleted();
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            this.FreeRentedBuffers();
        }

        /// <inheritdoc/>
        public void OnNext(in NmeaLineParser parsedLine, int lineNumber)
        {
            if (parsedLine.TagBlockAsciiWithoutDelimiters.Length > 0 &&
                parsedLine.TagBlock.SentenceGrouping.HasValue)
            {
                NmeaTagBlockSentenceGrouping sentenceGrouping = parsedLine.TagBlock.SentenceGrouping.Value;

                bool isLastSentenceInGroup = sentenceGrouping.SentenceNumber == sentenceGrouping.SentencesInGroup;

                if (!isLastSentenceInGroup && parsedLine.Padding != 0)
                {
                    this.messageProcessor.OnError(
                        parsedLine.Line,
                        new ArgumentException("Can only handle non-zero padding on the final message in a fragment"),
                        lineNumber);
                }

                int groupId = sentenceGrouping.GroupId;

                if (!this.messageFragments.TryGetValue(groupId, out FragmentedMessage fragments))
                {
                    fragments = new FragmentedMessage(
                        sentenceGrouping.SentencesInGroup,
                        lineNumber);
                    this.messageFragments.Add(groupId, fragments);
                }

                Span<byte[]> fragmentBuffers = fragments.Buffers;
                if (fragmentBuffers[sentenceGrouping.SentenceNumber - 1] != null)
                {
                    this.messageProcessor.OnError(
                        parsedLine.Line,
                        new ArgumentException($"Already received sentence {sentenceGrouping.SentenceNumber} for group {groupId}"),
                        lineNumber);
                }

                byte[] buffer = ArrayPool<byte>.Shared.Rent(parsedLine.Line.Length);
                fragmentBuffers[sentenceGrouping.SentenceNumber - 1] = buffer;
                parsedLine.Line.CopyTo(buffer);

                bool allFragmentsReceived = true;
                int totalPayloadSize = 0;

                for (int i = 0; i < fragmentBuffers.Length; ++i)
                {
                    if (fragmentBuffers[i] == null)
                    {
                        allFragmentsReceived = false;
                        break;
                    }

                    var storedParsedLine = new NmeaLineParser(fragmentBuffers[i], this.options.ThrowWhenTagBlockContainsUnknownFields);
                    totalPayloadSize += storedParsedLine.Payload.Length;
                }

                if (allFragmentsReceived)
                {
                    byte[] reassemblyUnderlyingArray = null;
                    try
                    {
                        reassemblyUnderlyingArray = ArrayPool<byte>.Shared.Rent(totalPayloadSize);
                        int reassemblyIndex = 0;
                        Span<byte> reassemblyBuffer = reassemblyUnderlyingArray.AsSpan().Slice(0, totalPayloadSize);
                        uint finalPadding = 0;

                        for (int i = 0; i < fragmentBuffers.Length; ++i)
                        {
                            var storedParsedLine = new NmeaLineParser(fragmentBuffers[i], this.options.ThrowWhenTagBlockContainsUnknownFields);
                            ReadOnlySpan<byte> payload = storedParsedLine.Payload;
                            payload.CopyTo(reassemblyBuffer.Slice(reassemblyIndex, payload.Length));
                            reassemblyIndex += payload.Length;
                            finalPadding = storedParsedLine.Padding;
                        }

                        this.messageProcessor.OnNext(
                            new NmeaLineParser(fragmentBuffers[0], this.options.ThrowWhenTagBlockContainsUnknownFields),
                            reassemblyBuffer.Slice(0, totalPayloadSize),
                            finalPadding);
                        this.messagesProcessed += 1;
                    }
                    finally
                    {
                        if (reassemblyUnderlyingArray != null)
                        {
                            ArrayPool<byte>.Shared.Return(reassemblyUnderlyingArray);
                        }
                    }

                    this.FreeMessageFragments(groupId);
                }
            }
            else
            {
                this.messageProcessor.OnNext(parsedLine, parsedLine.Payload, parsedLine.Padding);
                this.messagesProcessed += 1;
            }

            if (this.messageFragments.Count > 0)
            {
                Span<int> fragmentGroupIdsToRemove = stackalloc int[this.messageFragments.Count];
                int fragmentToRemoveCount = 0;
                foreach (KeyValuePair<int, FragmentedMessage> kv in this.messageFragments)
                {
                    int sentencesSinceFirstFragment = lineNumber - kv.Value.LineNumber;
                    if (sentencesSinceFirstFragment > this.options.MaximumUnmatchedFragmentAge)
                    {
                        fragmentGroupIdsToRemove[fragmentToRemoveCount++] = kv.Key;
                    }
                }

                for (int i = 0; i < fragmentToRemoveCount; ++i)
                {
                    int groupId = fragmentGroupIdsToRemove[i];
                    FragmentedMessage fragmentedMessage = this.messageFragments[groupId];
                    Span<byte[]> fragmentBuffers = fragmentedMessage.Buffers;

                    // Find the last non-null entry in the list. (It would be easier to use LINQ's
                    // Last operator, but this we way avoid the allocations inherent in Last,
                    // although since this is a case we don't expect to hit much in normal
                    // operation, it's not clear how much that matters.)
                    byte[] lastFragmentBuffer = null;
                    for (int o = fragmentBuffers.Length - 1; lastFragmentBuffer == null; --o)
                    {
                        lastFragmentBuffer = fragmentBuffers[o];
                    }

                    ReadOnlySpan<byte> line = lastFragmentBuffer;
                    int endOfMessages = line.IndexOf((byte)0);
                    if (endOfMessages >= 0)
                    {
                        line = line.Slice(0, endOfMessages);
                    }

                    this.messageProcessor.OnError(
                        line,
                        new ArgumentException("Received incomplete fragmented message."),
                        fragmentedMessage.LineNumber);

                    this.FreeMessageFragments(groupId);
                }
            }
        }

        /// <inheritdoc/>
        public void OnError(in ReadOnlySpan<byte> line, Exception error, int lineNumber)
        {
            this.messageProcessor.OnError(line, error, lineNumber);
        }

        /// <inheritdoc/>
        public void Progress(bool done, int totalLines, int totalTicks, int linesSinceLastUpdate, int ticksSinceLastUpdate)
        {
            this.messageProcessor.Progress(
                done,
                totalLines,
                this.messagesProcessed,
                totalTicks,
                linesSinceLastUpdate,
                this.messagesProcessed - this.messagesProcessedAtLastUpdate,
                ticksSinceLastUpdate);
            this.messagesProcessedAtLastUpdate = this.messagesProcessed;
        }

        private void FreeRentedBuffers()
        {
            if (this.messageFragments.Count > 0)
            {
                int[] groupIds = this.messageFragments.Keys.ToArray();
                Console.WriteLine($"{groupIds.Length} message groups with missing fragments");
                for (int i = 0; i < groupIds.Length; ++i)
                {
                    Console.WriteLine($"Partial message, group id {groupIds[i]}");

                    this.FreeMessageFragments(groupIds[i]);
                }
            }
        }

        private void FreeMessageFragments(int groupId)
        {
            FragmentedMessage fragmentedMessage = this.messageFragments[groupId];
            Span<byte[]> fragmentBuffers = fragmentedMessage.Buffers;
            for (int i = 0; i < fragmentBuffers.Length; ++i)
            {
                if (fragmentBuffers[i] != null)
                {
                    ArrayPool<byte>.Shared.Return(fragmentBuffers[i]);
                }
            }

            ArrayPool<byte[]>.Shared.Return(fragmentedMessage.RentedBufferArray);

            this.messageFragments.Remove(groupId);
        }

        private struct FragmentedMessage
        {
            public FragmentedMessage(
                int count,
                int lineNumber)
            {
                this.RentedBufferArray = ArrayPool<byte[]>.Shared.Rent(count);
                this.BufferCount = count;
                this.LineNumber = lineNumber;

                this.Buffers.Clear();
            }

            /// <summary>
            /// Gets the underlying array provided by the array pool that backs <see cref="Buffer"/>.
            /// </summary>
            /// <remarks>
            /// This might be larger than we need, because array pool only guarantees to return arrays
            /// that are at least as large as you want. The <see cref="Buffer"/> property provides
            /// correctly range-limited access via a span, but we need to hold onto the underlying
            /// array directly so that we can return it to the pool when we're done.
            /// </remarks>
            public byte[][] RentedBufferArray { get; }

            public int BufferCount { get; }

            /// <summary>
            /// Gets the span for holding references to the buffers for the fragments of this
            /// message.
            /// </summary>
            /// <remarks>
            /// <para>
            /// The entries in this span are null after construction, with buffers obtained for
            /// each fragment as they arrive. (The first will be put in place immediately after
            /// construction, because we only know that we've got a fragmented message as a result
            /// of seeing one of its fragments. But any further fragments get added as they
            /// arrive.)
            /// </para>
            /// <para>
            /// The byte arrays in here are all obtained from <see cref="ArrayPool{T}.Shared"/>,
            /// which has two implications. First, it means we must eventually return them to the
            /// pool, which happens in <see cref="FreeMessageFragments(int)"/>. Second, it means
            /// that each array may well be large than required. This is also the case for the
            /// underlying <see cref="RentedBufferArray"/> that backs the span returned by this
            /// property, but unlike here, where we range-limit the span based on the number of
            /// buffers (<see cref="BufferCount"/>) we don't keep track of the real length of
            /// each individual fragment. That's because we don't have to: we can infer it by
            /// inspecting the fragment contents. When the time for reassembly comes (i.e., once
            /// all the fragments have arrived), we use a <see cref="NmeaLineParser"/> to pull
            /// out the fragment payload, and the parser doesn't care if the buffer containing
            /// the line is longer than it needs to be.
            /// </para>
            /// </remarks>
            public Span<byte[]> Buffers => this.RentedBufferArray.AsSpan().Slice(0, this.BufferCount);

            public int LineNumber { get; }
        }
    }
}
