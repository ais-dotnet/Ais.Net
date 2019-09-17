using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ais.Net.Benchmarks
{
    public static class InspectMessageType
    {
        private static TestProcessor processor = new TestProcessor();

        public static async Task Process1000MessageFromFile()
        {
            await NmeaStreamParser.ParseFileAsync(@"TestData\Ais1000Lines.nm4", processor);
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
