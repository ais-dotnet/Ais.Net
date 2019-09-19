using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ais.Net.Benchmarks
{
    internal static class ReadAllPositions
    {
        private static StatsScanner processor = new StatsScanner();

        public static async Task Process1000MessagesFromFile(string path)
        {
            await NmeaStreamParser.ParseFileAsync(path, processor);
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
                    //this.SummedLats += parsedPosition.Latitude10000thMins;
                    //this.SummedLongs += parsedPosition.Longitude10000thMins;
                    //this.PositionsCount += 1;
                }
                else if (messageType == 18)
                {
                    var parsedPosition = new NmeaAisPositionReportClassBParser(asciiPayload, padding);
                    AddPosition(parsedPosition.Latitude10000thMins, parsedPosition.Longitude10000thMins);
                    //this.SummedLats += parsedPosition.Latitude10000thMins;
                    //this.SummedLongs += parsedPosition.Longitude10000thMins;
                    //this.PositionsCount += 1;
                }
                else if (messageType == 19)
                {
                    var parsedPosition = new NmeaAisPositionReportExtendedClassBParser(asciiPayload, padding);
                    AddPosition(parsedPosition.Latitude10000thMins, parsedPosition.Longitude10000thMins);
                    //this.SummedLats += parsedPosition.Latitude10000thMins;
                    //this.SummedLongs += parsedPosition.Longitude10000thMins;
                    //this.PositionsCount += 1;
                }

                void AddPosition(int latitude10000thMins, int Longitude10000thMins)
                {
                    this.SummedLats += latitude10000thMins;
                    this.SummedLongs += Longitude10000thMins;
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
        }
    }
}
