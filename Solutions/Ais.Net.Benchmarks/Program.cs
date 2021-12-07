// <copyright file="Program.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Ais.Net.Benchmarks
{
    using System.IO;
    using BenchmarkDotNet.Configs;
    using BenchmarkDotNet.Diagnosers;
    using BenchmarkDotNet.Jobs;
    using BenchmarkDotNet.Running;
    using BenchmarkDotNet.Toolchains.InProcess.Emit;

    /// <summary>
    /// Program entry point type.
    /// </summary>
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
        private static void Main(string[] args)
        {
            Job job = Job.Default;
            IConfig config = DefaultConfig.Instance
                .AddDiagnoser(MemoryDiagnoser.Default);

            if (args.Length == 1 && args[0] == "inprocess")
            {
                job = job
                        .WithMinWarmupCount(2)
                        .WithMaxWarmupCount(4)
                        .WithToolchain(InProcessEmitToolchain.Instance);
                config = config.WithOptions(ConfigOptions.DisableOptimizationsValidator);
            }
            else
            {
                if (args.Length > 0)
                {
                    string artifactsPath = args[0];
                    Directory.CreateDirectory(artifactsPath);
                    config = config.WithArtifactsPath(artifactsPath);
                }

                if (args.Length > 1)
                {
                    string version = args[1];
                    job = job.WithArguments(new Argument[] { new MsBuildArgument($"/p:Version={version}") });
                }
            }

            config = config.AddJob(job);

            BenchmarkRunner.Run<AisNetBenchmarks>(config);
        }
    }
}
