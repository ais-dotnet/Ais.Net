// <copyright file="InspectMessageType.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Ais.Net.Benchmarks
{
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Benchmark that measures how quickly we can read messages from a file and discover their
    /// types.
    /// </summary>
    internal static class InspectMessageType
    {
        private static readonly TestProcessor Processor = new TestProcessor();

        /// <summary>
        /// Execute the benchmark.
        /// </summary>
        /// <param name="path">The file from which to read messages.</param>
        /// <returns>A task that completes when the benchmark has finished.</returns>
        public static async Task ProcessMessagesFromFile(string path)
        {
            await NmeaStreamParser.ParseFileAsync(path, Processor).ConfigureAwait(false);
        }

        private class TestProcessor : INmeaAisMessageStreamProcessor
        {
            private readonly int[] messageTypeCounts = new int[30];

            public void OnCompleted()
            {
            }

            public void OnError(in ReadOnlySpan<byte> line, Exception error, int lineNumber)
            {
            }

            public void OnNext(in NmeaLineParser firstLine, in ReadOnlySpan<byte> asciiPayload, uint padding)
            {
                int type = NmeaPayloadParser.PeekMessageType(asciiPayload, padding);
                if (type < this.messageTypeCounts.Length)
                {
                    this.messageTypeCounts[type] += 1;
                }
            }

            public void Progress(bool done, int totalNmeaLines, int totalAisMessages, int totalTicks, int nmeaLinesSinceLastUpdate, int aisMessagesSinceLastUpdate, int ticksSinceLastUpdate)
            {
            }
        }
    }
}
