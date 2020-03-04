// <copyright file="ReadAllPositions.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Ais.Net.Benchmarks
{
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Benchmark that measures how quickly we can read messages from a file and read out any
    /// location data they contain.
    /// </summary>
    internal static class ReadAllPositions
    {
        private static readonly StatsScanner Processor = new StatsScanner();

        /// <summary>
        /// Execute the benchmark.
        /// </summary>
        /// <param name="path">The file from which to read messages.</param>
        /// <returns>A task that completes when the benchmark has finished.</returns>
        public static async Task ProcessMessagesFromFile(string path)
        {
            await NmeaStreamParser.ParseFileAsync(path, Processor).ConfigureAwait(false);
        }

        private class StatsScanner : INmeaAisMessageStreamProcessor
        {
            public long SummedLongs { get; private set; } = 0;

            public long SummedLats { get; private set; } = 0;

            public int PositionsCount { get; private set; } = 0;

            /// <inheritdoc/>
            public void OnNext(
                in NmeaLineParser firstLine,
                in ReadOnlySpan<byte> asciiPayload,
                uint padding)
            {
                int messageType = NmeaPayloadParser.PeekMessageType(asciiPayload, padding);
                if (messageType >= 1 && messageType <= 3)
                {
                    var parsedPosition = new NmeaAisPositionReportClassAParser(asciiPayload, padding);
                    AddPosition(parsedPosition.Latitude10000thMins, parsedPosition.Longitude10000thMins);
                }
                else if (messageType == 18)
                {
                    var parsedPosition = new NmeaAisPositionReportClassBParser(asciiPayload, padding);
                    AddPosition(parsedPosition.Latitude10000thMins, parsedPosition.Longitude10000thMins);
                }
                else if (messageType == 19)
                {
                    var parsedPosition = new NmeaAisPositionReportExtendedClassBParser(asciiPayload, padding);
                    AddPosition(parsedPosition.Latitude10000thMins, parsedPosition.Longitude10000thMins);
                }

                void AddPosition(int latitude10000thMins, int longitude10000thMins)
                {
                    this.SummedLats += latitude10000thMins;
                    this.SummedLongs += longitude10000thMins;
                    this.PositionsCount += 1;
                }
            }

            /// <inheritdoc/>
            public void OnCompleted()
            {
            }

            /// <inheritdoc/>
            public void Progress(
                bool done,
                int totalNmeaLines,
                int totalAisMessages,
                int totalTicks,
                int nmeaLinesSinceLastUpdate,
                int aisMessagesSinceLastUpdate,
                int ticksSinceLastUpdate)
            {
            }

            public void OnError(in ReadOnlySpan<byte> line, Exception error, int lineNumber)
            {
            }
        }
    }
}
