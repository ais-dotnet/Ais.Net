using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ais.Net.Benchmarks
{
    internal static class InspectMessageType
    {
        private static TestProcessor processor = new TestProcessor();

        public static async Task ProcessMessagesFromFile(string path)
        {
            await NmeaStreamParser.ParseFileAsync(path, processor);
        }

        private class TestProcessor : INmeaAisMessageStreamProcessor
        {
            private int[] messageTypeCounts = new int[30];

            public void OnCompleted()
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
