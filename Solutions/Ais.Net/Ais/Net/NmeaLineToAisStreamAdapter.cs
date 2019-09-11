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
        private readonly Dictionary<int, IMemoryOwner<byte>[]> messageFragments = new Dictionary<int, IMemoryOwner<byte>[]>();
        private int messagesProcessed = 0;
        private int messagesProcessedAtLastUpdate = 0;

        /// <summary>
        /// Creates a <see cref="NmeaLineToAisStreamAdapter"/>.
        /// </summary>
        /// <param name="messageProcessor">
        /// The message process to which complete AIS messages are to be passed.
        /// </param>
        public NmeaLineToAisStreamAdapter(INmeaAisMessageStreamProcessor messageProcessor)
        {
            this.messageProcessor = messageProcessor;
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
        public void OnNext(in NmeaLineParser parsedLine)
        {
            if (parsedLine.TagBlockAsciiWithoutDelimiters.Length > 0 &&
                parsedLine.TagBlock.SentenceGrouping.HasValue)
            {
                NmeaTagBlockSentenceGrouping sentenceGrouping = parsedLine.TagBlock.SentenceGrouping.Value;

                bool isLastSentenceInGroup = sentenceGrouping.SentenceNumber == sentenceGrouping.SentencesInGroup;

                if (!isLastSentenceInGroup && parsedLine.Padding != 0)
                {
                    throw new ArgumentException("Can only handle non-zero padding on the final message in a fragment");
                }

                int groupId = sentenceGrouping.GroupId;

                if (!this.messageFragments.TryGetValue(groupId, out IMemoryOwner<byte>[] fragments))
                {
                    fragments = new IMemoryOwner<byte>[sentenceGrouping.SentencesInGroup];
                    this.messageFragments.Add(groupId, fragments);
                }

                if (fragments[sentenceGrouping.SentenceNumber - 1] != null)
                {
                    throw new ArgumentException($"Already received sentence {sentenceGrouping.SentenceNumber} for group {groupId}");
                }

                IMemoryOwner<byte> buffer = MemoryPool<byte>.Shared.Rent(parsedLine.Line.Length);
                fragments[sentenceGrouping.SentenceNumber - 1] = buffer;
                parsedLine.Line.CopyTo(buffer.Memory.Span);

                bool allFragmentsReceived = true;
                int totalPayloadSize = 0;

                for (int i = 0; i < fragments.Length; ++i)
                {
                    if (fragments[i] == null)
                    {
                        allFragmentsReceived = false;
                        break;
                    }

                    var storedParsedLine = new NmeaLineParser(fragments[i].Memory.Span);
                    totalPayloadSize += storedParsedLine.Payload.Length;
                }

                if (allFragmentsReceived)
                {
                    using (IMemoryOwner<byte> reassembly = MemoryPool<byte>.Shared.Rent(totalPayloadSize))
                    {
                        int reassemblyIndex = 0;
                        Span<byte> reassemblyBuffer = reassembly.Memory.Span;
                        uint finalPadding = 0;

                        for (int i = 0; i < fragments.Length; ++i)
                        {
                            var storedParsedLine = new NmeaLineParser(fragments[i].Memory.Span);
                            ReadOnlySpan<byte> payload = storedParsedLine.Payload;
                            payload.CopyTo(reassemblyBuffer.Slice(reassemblyIndex, payload.Length));
                            reassemblyIndex += payload.Length;
                            finalPadding = storedParsedLine.Padding;
                        }

                        this.messageProcessor.OnNext(
                            new NmeaLineParser(fragments[0].Memory.Span),
                            reassemblyBuffer.Slice(0, totalPayloadSize),
                            finalPadding);
                        this.messagesProcessed += 1;
                    }

                    this.FreeMessageFragments(groupId);
                }
            }
            else
            {
                this.messageProcessor.OnNext(parsedLine, parsedLine.Payload, parsedLine.Padding);
                this.messagesProcessed += 1;
            }
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
            IMemoryOwner<byte>[] fragments = this.messageFragments[groupId];
            for (int i = 0; i < fragments.Length; ++i)
            {
                fragments[i]?.Dispose();
            }

            this.messageFragments.Remove(groupId);
        }
    }
}
