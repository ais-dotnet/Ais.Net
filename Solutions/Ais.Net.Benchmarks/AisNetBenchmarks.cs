// <copyright file="AisNetBenchmarks.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Ais.Net.Benchmarks
{
    using System.IO;
    using System.Threading.Tasks;
    using BenchmarkDotNet.Attributes;

    /// <summary>
    /// Defines all of the benchmarks and global setup/teardown.
    /// </summary>
    [JsonExporterAttribute.Full]
    public class AisNetBenchmarks
    {
        private const string TestPath1kLines = "TestData/Ais1000Lines.nm4";
        private const string TestPath1mLines = "TestData/Ais1000000Lines.nm4";

        /// <summary>
        /// Invoked by BenchmarkDotNet before running all benchmarks.
        /// </summary>
        [GlobalSetup]
        public void GlobalSetup()
        {
            // We have 1000 lines of real test data to provide a realistic mix of messages.
            // However, this is too small to get a good measurement of the per-message overhead,
            // because the per-execution overheads are a significant proportion of the whole at
            // that size.
            // For example, on an Intel Core i9-9900K CPU 3.60GHz, the InspectMessageType test
            // processes a 1000-message file in about 510us, suggesting a per-message cost of
            // about 510ns. However, if we run the exact same test against a 1,000,000 message
            // file, it takes about 340ms, suggesting a per-message cost of just 340ns. So by
            // measuring over 1,000 messages, we get a reading that's 50% higher than we do at
            // 1M. (We get similar results at 10M, so 1M seems to be sufficient. 100K might also
            // be enough, but it's easier to read the results when we multiple things up three
            // orders of magnitude at a time: it means that test times in ms correspond to
            // per-message times in ns.)
            string[] testFileLines = File.ReadAllLines(TestPath1kLines);
            using var f = new StreamWriter(TestPath1mLines);
            for (int i = 0; i < 1000; ++i)
            {
                foreach (string line in testFileLines)
                {
                    f.WriteLine(line);
                }
            }
        }

        /// <summary>
        /// Invoked by BenchmarkDotNet after running all benchmarks.
        /// </summary>
        [GlobalCleanup]
        public void GlobalCleanup()
        {
            File.Delete(TestPath1mLines);
        }

        /// <summary>
        /// Benchmark: measure the speed at which we can perform the most minimal amount of
        /// processing of messages in a file.
        /// </summary>
        /// <returns>A task that completes when the benchmark has finished.</returns>
        [Benchmark]
        public Task InspectMessageTypesFromNorwayFile1M() => InspectMessageType.ProcessMessagesFromFile(TestPath1mLines);

        /// <summary>
        /// Benchmark: measure the speed at which we can read location data from message in a file.
        /// </summary>
        /// <returns>A task that completes when the benchmark has finished.</returns>
        [Benchmark]
        public Task ReadPositionsFromNorwayFile1M() => ReadAllPositions.ProcessMessagesFromFile(TestPath1mLines);
    }
}
