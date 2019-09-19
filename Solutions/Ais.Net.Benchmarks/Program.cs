using System.IO;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;

namespace Ais.Net.Benchmarks
{
    internal static class Program
    {
        /// <summary>
        /// Program entry point.
        /// </summary>
        /// <param name="args">Command line arguments.</param>
        /// <remarks>
        /// <p>
        /// When running in an Azure DevOps pipeline, a couple of things can go wrong with BenchmarkDotNet.
        /// First, by default it puts its output into a path relative to the current working directory,
        /// which will often be some way further up the folder hierarchy than we want. Second, the components
        /// under test will have been built with a /p:Version=x.y.z argument, setting the version number to
        /// whatever the build process has determined it should be. That can be problematic because
        /// BenchmarkDotNet rebuilds various elements for each benchmark, meaning everything would default back
        /// to v1.0.0.0. However, that seems to cause problems because the hosting benchmark project will
        /// have been build against the correct version number. This shouldn't be a problem because the
        /// benchmarks all run isolated, but weirdly, we get an error *after* the benchmarking is complete,
        /// causing this hosting program to exit with an error.
        /// </p>
        /// <p>
        /// To fix these problems, this application accepts two command line arguments. If present, they
        /// set the path of the folder into which to write results, and the version number to be used when
        /// rebuilding things.
        /// </p>
        /// </remarks>
        static void Main(string[] args)
        {
            IConfig config = DefaultConfig.Instance.With(MemoryDiagnoser.Default);
            if (args.Length > 0)
            {
                string artifactsPath = args[0];
                Directory.CreateDirectory(artifactsPath);
                config = config.WithArtifactsPath(artifactsPath);
            }

            if (args.Length > 1)
            {
                string version = args[1];
                config = config.With(Job.Default.With(new Argument[] { new MsBuildArgument($"/p:Version={version}") }));
            }

            BenchmarkRunner.Run<AllBenchmarks>(config);
        }
    }
}
