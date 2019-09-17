using System;
using BenchmarkDotNet.Running;

namespace Ais.Net.Benchmarks
{
    class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<AllBenchmarks>();
        }
    }
}
