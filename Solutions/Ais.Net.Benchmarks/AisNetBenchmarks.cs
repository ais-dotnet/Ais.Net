using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;

namespace Ais.Net.Benchmarks
{
    [JsonExporterAttribute.Full]
    public class AllBenchmarks
    {
        [Benchmark]
        public Task InspectMessageTypesFromFile1000() => InspectMessageType.Process1000MessageFromFile();
    }
}
