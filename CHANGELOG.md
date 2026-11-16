# Changelog

All notable changes to the `Ais.Net` package are documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

### Changed

- The package now depends on `System.IO.Pipelines` 11.0.0 or later (0.4.3 depended on 4.7.4). ([#206](https://github.com/ais-dotnet/Ais.Net/pull/206), [#209](https://github.com/ais-dotnet/Ais.Net/pull/209))
- On .NET Standard 2.0, the package now depends on `System.Memory` 4.6.3 or later (0.4.3 depended on 4.5.4). ([#206](https://github.com/ais-dotnet/Ais.Net/pull/206))
- CI builds of the package are deterministic. Source Link now comes from the .NET SDK instead of the `Microsoft.SourceLink.GitHub` package. ([#209](https://github.com/ais-dotnet/Ais.Net/pull/209))
- The library builds with the .NET 10 SDK. Some internal methods now use `scoped` parameters, to follow the newer C# ref safety rules. The public API does not change. ([#189](https://github.com/ais-dotnet/Ais.Net/pull/189))
- The specs and benchmarks target .NET 10 only, because .NET 10 is the only LTS release still in support. The library still targets .NET Standard 2.0 and .NET Standard 2.1. ([#209](https://github.com/ais-dotnet/Ais.Net/pull/209))
- The specs use Reqnroll instead of SpecFlow. ([#201](https://github.com/ais-dotnet/Ais.Net/pull/201))
- The build uses [ZeroFailed](https://github.com/zerofailed/ZeroFailed), and runs as a single GitHub Actions job. Releases publish to nuget.org with Trusted Publishing. ([#208](https://github.com/ais-dotnet/Ais.Net/pull/208))
- The solution uses the `.slnx` format, Central Package Management and a shared `Directory.Build.props` file. ([#209](https://github.com/ais-dotnet/Ais.Net/pull/209))

## [0.4.3] - 2021-12-10

### Removed

- The `netcoreapp2.1` target. .NET Core 2.1 and later use the .NET Standard 2.1 target, which gives the same performance. ([#142](https://github.com/ais-dotnet/Ais.Net/pull/142))

### Changed

- The specs and benchmarks target .NET 6.0. ([#142](https://github.com/ais-dotnet/Ais.Net/pull/142))

## [0.4.2] - 2021-06-18

### Fixed

- Typos in the documentation comments for `NmeaAisLongRangeAisBroadcastParser` and `ShipType`, and in the README. ([#128](https://github.com/ais-dotnet/Ais.Net/pull/128))

## [0.4.1] - 2021-03-02

### Fixed

- `NmeaLineToAisStreamAdapter` reassembles fragmented messages when the NMEA tag block has no sentence group (`g`) field. In this case, it uses the fragment information in the AIS sentence. ([#126](https://github.com/ais-dotnet/Ais.Net/pull/126))

## [0.4.0] - 2021-02-15

### Changed

- The package depends on `System.IO.Pipelines` 4.7.4 (was 4.5.3), and on .NET Standard 2.0, on `System.Memory` 4.5.4 (was 4.5.3). ([#118](https://github.com/ais-dotnet/Ais.Net/pull/118))

## [0.3.1] - 2021-02-03

### Added

- `INmeaLineStreamProcessor.OnError` and `INmeaAisMessageStreamProcessor.OnError`. The stream parser reports a malformed line or message through `OnError`, and then continues with the rest of the stream. ([#51](https://github.com/ais-dotnet/Ais.Net/pull/51))
- `NmeaParserOptions.ThrowWhenTagBlockContainsUnknownFields`. Set it to `false` to ignore non-standard tag block fields. The default is `true`. ([#52](https://github.com/ais-dotnet/Ais.Net/pull/52))
- `NmeaParserOptions.MaximumUnmatchedFragmentAge`. It sets how many lines the parser reads while it waits for the remaining fragments of a message. After that, it reports the incomplete message as an error. The default is 8. ([#56](https://github.com/ais-dotnet/Ais.Net/pull/56))
- The package includes its icon. ([#115](https://github.com/ais-dotnet/Ais.Net/pull/115))

### Changed

- The error message for a truncated NMEA message describes the problem more clearly. ([#53](https://github.com/ais-dotnet/Ais.Net/pull/53))
- Fragmented message reassembly uses `ArrayPool<T>` instead of `MemoryPool<T>`, which removes an allocation for each fragment. ([#83](https://github.com/ais-dotnet/Ais.Net/pull/83))

## [0.3.0] - 2020-02-03

### Added

- A .NET Standard 2.1 target, which uses the `Memory<T>` overloads of `Stream.Read` for better performance. The .NET Standard 2.0 and `netcoreapp2.1` targets remain. ([#37](https://github.com/ais-dotnet/Ais.Net/pull/37))
- Benchmarks. ([#8](https://github.com/ais-dotnet/Ais.Net/pull/8))
- Introductory documentation, and API documentation built with DocFX. ([#11](https://github.com/ais-dotnet/Ais.Net/pull/11), [#16](https://github.com/ais-dotnet/Ais.Net/pull/16))

## [0.2.0] - 2019-09-13

### Changed

- The table format in the README. ([#5](https://github.com/ais-dotnet/Ais.Net/pull/5))

## [0.1.0] - 2019-09-13

### Added

- First release. The library parses NMEA AIVDM and AIVDO sentences, and decodes the AIS message types listed in the README, without allocating memory for each message.

[Unreleased]: https://github.com/ais-dotnet/Ais.Net/compare/0.4.3...HEAD
[0.4.3]: https://github.com/ais-dotnet/Ais.Net/compare/0.4.2...0.4.3
[0.4.2]: https://github.com/ais-dotnet/Ais.Net/compare/0.4.1...0.4.2
[0.4.1]: https://github.com/ais-dotnet/Ais.Net/compare/0.4.0...0.4.1
[0.4.0]: https://github.com/ais-dotnet/Ais.Net/compare/0.3.1...0.4.0
[0.3.1]: https://github.com/ais-dotnet/Ais.Net/compare/0.3.0...0.3.1
[0.3.0]: https://github.com/ais-dotnet/Ais.Net/compare/0.2.0...0.3.0
[0.2.0]: https://github.com/ais-dotnet/Ais.Net/compare/0.1.0...0.2.0
[0.1.0]: https://github.com/ais-dotnet/Ais.Net/releases/tag/0.1.0
