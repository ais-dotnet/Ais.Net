# Ais.Net
[![#](https://img.shields.io/nuget/v/Ais.Net.svg)](https://www.nuget.org/packages/Ais.Net/) [![Build Status](https://dev.azure.com/endjin-labs/Ais.Net/_apis/build/status/ais-dotnet.Ais.Net?branchName=master)](https://dev.azure.com/endjin-labs/Ais.Net/_build/latest?definitionId=2&branchName=master)
[![GitHub license](https://img.shields.io/badge/License-AGPL%20v3-blue.svg)](https://raw.githubusercontent.com/ais-dotnet/Ais.Net/master/LICENSE)
[![GitHub license](https://img.shields.io/badge/License-NLOD-blue.svg)](https://data.norge.no/nlod/en/2.0)
[![IMM](https://endimmfuncdev.azurewebsites.net/api/imm/github/ais-dotnet/Ais.Net/total?cache=false)](https://endimmfuncdev.azurewebsites.net/api/imm/github/ais-dotnet/Ais.Net/total?cache=false)

.NET Standard, high performance, zero allocation AIS decoder, which can process millions of AIVDM/AIVDO sentences per second on a single core. Documentation is available at https://ais-dotnet.github.io/Ais.Net/

## Supported Message Types

This library has been developed to support specific applications, so it only supports message types that have been needed to date in those applications. The following message types are currently supported.

| ID(s)   | Name                                |
|---------|-------------------------------------|
| 1, 2, 3 | Position Report Class A             |
| 5       | Static Voyage Related Data          |
| 18      | Standard Class B CS Position Report |
| 19      | Extended Class B CS Position Report |
| 24      | Static Data Report                  |
| 27      | Long Range AIS Broadcast message    |

See https://gpsd.gitlab.io/gpsd/AIVDM.html for a complete list of message types.

## Performance

The benchmark suite is a work in progress, but it currently measures two basic scenarios: discovering
message types, and extracting position data. Here are the results of executing these benchmarks on a system
with an Intel Core i9-9900K CPU 3.60GHz CPU:

```
|                              Method |     Mean |    Error |   StdDev |     Gen 0 | Gen 1 | Gen 2 | Allocated |
|------------------------------------ |---------:|---------:|---------:|----------:|------:|------:|----------:|
| InspectMessageTypesFromNorwayFile1M | 347.5 ms | 3.960 ms | 3.704 ms | 1000.0000 |     - |     - |   3.57 KB |
|       ReadPositionsFromNorwayFile1M | 403.5 ms | 5.395 ms | 5.046 ms | 1000.0000 |     - |     - |   3.57 KB |
```

The test processes 1 million messages, so to get the per-message timings, we divide by 1 million (so
`ms` in this table indicate ns per message). This demonstrates a per-message cost of 404ns to extract the
position data, or 348ns just to inspect the message type. Note that these tests read data from a file, so
this includes all the IO overhead involved in getting hold of the data to be processed. (These messages are
in NMEA format by the way.)

Note that the `1000.0000` Gen 0 GCs is slightly misleading. BenchmarkDotNet reports the number of GCs per
1,000 executions of the benchmark. But in this case the benchmarks take long enough that it only executes
them once each per iteration. The number it shows is the number of GCs multiplied by 1,000, then divided by
the number of times it executed the benchmark (i.e. 1), so what this actually shows is that there was a single GC. We seem to see this whether we parse 1,000, 1,000,000, or 10,000,000 messages, so although we're
not quite sure why we get an GC at all, the important point here is that the memory overhead is fixed, no
matter how many messages you process.


## Licenses

[![GitHub license](https://img.shields.io/badge/License-AGPL%20v3-blue.svg)](https://raw.githubusercontent.com/ais-dotnet/Ais.Net/master/LICENSE)

Ais.Net is available under the [GNU Affero General Public License v3.0 (GNU AGPLv3)](https://choosealicense.com/licenses/agpl-3.0/) open source license. The AGPLv3 is a Copyleft license, and is ideal for use cases such as open source projects with open source distribution, student/academic purposes, hobby projects, internal research projects without external distribution, or other projects where all GNU Affero General Public License v3.0 obligations can be met. 

Ais.Net is also available under a commercial license, which gives you the full rights to create and distribute software on your own terms without any open source license obligations. With the commercial license you also have access to official Ais.Net support and close strategic relationship with endjin to ensure your development goals are met.

For any licensing questions, please email [&#108;&#105;&#99;&#101;&#110;&#115;&#105;&#110;&#103;&#64;&#101;&#110;&#100;&#106;&#105;&#110;&#46;&#99;&#111;&#109;](&#109;&#97;&#105;&#108;&#116;&#111;&#58;&#108;&#105;&#99;&#101;&#110;&#115;&#105;&#110;&#103;&#64;&#101;&#110;&#100;&#106;&#105;&#110;&#46;&#99;&#111;&#109;)

[![GitHub license](https://img.shields.io/badge/License-NLOD-blue.svg)](https://data.norge.no/nlod/en/2.0)

The data used by the Ais.Net executable specifications and samples is licensed under the [Norwegian license for public data (NLOD)](https://data.norge.no/nlod/en/2.0).

## Project Sponsor

This project is sponsored by [endjin](https://endjin.com), a UK based Microsoft Gold Partner for Cloud Platform, Data Platform, Data Analytics, DevOps, and a Power BI Partner.

For more information about our products and services, or for commercial support of this project, please [contact us](https://endjin.com/contact-us). 

We produce two free weekly newsletters; [Azure Weekly](https://azureweekly.info) for all things about the Microsoft Azure Platform, and [Power BI Weekly](https://powerbiweekly.info).

Keep up with everything that's going on at endjin via our [blog](https://blogs.endjin.com/), follow us on [Twitter](https://twitter.com/endjin), or [LinkedIn](https://www.linkedin.com/company/1671851/).

Our other Open Source projects can be found at [https://endjin.com/open-source](https://endjin.com/open-source)

## Code of conduct

This project has adopted a code of conduct adapted from the [Contributor Covenant](http://contributor-covenant.org/) to clarify expected behavior in our community. This code of conduct has been [adopted by many other projects](http://contributor-covenant.org/adopters/). For more information see the [Code of Conduct FAQ](https://opensource.microsoft.com/codeofconduct/faq/) or contact [&#104;&#101;&#108;&#108;&#111;&#064;&#101;&#110;&#100;&#106;&#105;&#110;&#046;&#099;&#111;&#109;](&#109;&#097;&#105;&#108;&#116;&#111;:&#104;&#101;&#108;&#108;&#111;&#064;&#101;&#110;&#100;&#106;&#105;&#110;&#046;&#099;&#111;&#109;) with any additional questions or comments.

## IP Maturity Matrix (IMM)

The IMM is endjin's IP quality framework.

[![Shared Engineering Standards](https://endimmfuncdev.azurewebsites.net/api/imm/github/ais-dotnet/Ais.Net/rule/74e29f9b-6dca-4161-8fdd-b468a1eb185d?nocache=true)](https://endimmfuncdev.azurewebsites.net/api/imm/github/ais-dotnet/Ais.Net/rule/74e29f9b-6dca-4161-8fdd-b468a1eb185d?cache=false)

[![Coding Standards](https://endimmfuncdev.azurewebsites.net/api/imm/github/ais-dotnet/Ais.Net/rule/f6f6490f-9493-4dc3-a674-15584fa951d8?cache=false)](https://endimmfuncdev.azurewebsites.net/api/imm/github/ais-dotnet/Ais.Net/rule/f6f6490f-9493-4dc3-a674-15584fa951d8?cache=false)

[![Executable Specifications](https://endimmfuncdev.azurewebsites.net/api/imm/github/ais-dotnet/Ais.Net/rule/bb49fb94-6ab5-40c3-a6da-dfd2e9bc4b00?cache=false)](https://endimmfuncdev.azurewebsites.net/api/imm/github/ais-dotnet/Ais.Net/rule/bb49fb94-6ab5-40c3-a6da-dfd2e9bc4b00?cache=false)

[![Code Coverage](https://endimmfuncdev.azurewebsites.net/api/imm/github/ais-dotnet/Ais.Net/rule/0449cadc-0078-4094-b019-520d75cc6cbb?cache=false)](https://endimmfuncdev.azurewebsites.net/api/imm/github/ais-dotnet/Ais.Net/rule/0449cadc-0078-4094-b019-520d75cc6cbb?cache=false)

[![Benchmarks](https://endimmfuncdev.azurewebsites.net/api/imm/github/ais-dotnet/Ais.Net/rule/64ed80dc-d354-45a9-9a56-c32437306afa?cache=false)](https://endimmfuncdev.azurewebsites.net/api/imm/github/ais-dotnet/Ais.Net/rule/64ed80dc-d354-45a9-9a56-c32437306afa?cache=false)

[![Reference Documentation](https://endimmfuncdev.azurewebsites.net/api/imm/github/ais-dotnet/Ais.Net/rule/2a7fc206-d578-41b0-85f6-a28b6b0fec5f?cache=false)](https://endimmfuncdev.azurewebsites.net/api/imm/github/ais-dotnet/Ais.Net/rule/2a7fc206-d578-41b0-85f6-a28b6b0fec5f?cache=false)

[![Design & Implementation Documentation](https://endimmfuncdev.azurewebsites.net/api/imm/github/ais-dotnet/Ais.Net/rule/f026d5a2-ce1a-4e04-af15-5a35792b164b?cache=false)](https://endimmfuncdev.azurewebsites.net/api/imm/github/ais-dotnet/Ais.Net/rule/f026d5a2-ce1a-4e04-af15-5a35792b164b?cache=false)

[![How-to Documentation](https://endimmfuncdev.azurewebsites.net/api/imm/github/ais-dotnet/Ais.Net/rule/145f2e3d-bb05-4ced-989b-7fb218fc6705?cache=false)](https://endimmfuncdev.azurewebsites.net/api/imm/github/ais-dotnet/Ais.Net/rule/145f2e3d-bb05-4ced-989b-7fb218fc6705?cache=false)

[![Date of Last IP Review](https://endimmfuncdev.azurewebsites.net/api/imm/github/ais-dotnet/Ais.Net/rule/da4ed776-0365-4d8a-a297-c4e91a14d646?cache=false)](https://endimmfuncdev.azurewebsites.net/api/imm/github/ais-dotnet/Ais.Net/rule/da4ed776-0365-4d8a-a297-c4e91a14d646?cache=false)

[![Framework Version](https://endimmfuncdev.azurewebsites.net/api/imm/github/ais-dotnet/Ais.Net/rule/6c0402b3-f0e3-4bd7-83fe-04bb6dca7924?cache=false)](https://endimmfuncdev.azurewebsites.net/api/imm/github/ais-dotnet/Ais.Net/rule/6c0402b3-f0e3-4bd7-83fe-04bb6dca7924?cache=false)

[![Associated Work Items](https://endimmfuncdev.azurewebsites.net/api/imm/github/ais-dotnet/Ais.Net/rule/79b8ff50-7378-4f29-b07c-bcd80746bfd4?cache=false)](https://endimmfuncdev.azurewebsites.net/api/imm/github/ais-dotnet/Ais.Net/rule/79b8ff50-7378-4f29-b07c-bcd80746bfd4?cache=false)

[![Source Code Availability](https://endimmfuncdev.azurewebsites.net/api/imm/github/ais-dotnet/Ais.Net/rule/30e1b40b-b27d-4631-b38d-3172426593ca?cache=false)](https://endimmfuncdev.azurewebsites.net/api/imm/github/ais-dotnet/Ais.Net/rule/30e1b40b-b27d-4631-b38d-3172426593ca?cache=false)

[![License](https://endimmfuncdev.azurewebsites.net/api/imm/github/ais-dotnet/Ais.Net/rule/d96b5bdc-62c7-47b6-bcc4-de31127c08b7?cache=false)](https://endimmfuncdev.azurewebsites.net/api/imm/github/ais-dotnet/Ais.Net/rule/d96b5bdc-62c7-47b6-bcc4-de31127c08b7?cache=false)

[![Production Use](https://endimmfuncdev.azurewebsites.net/api/imm/github/ais-dotnet/Ais.Net/rule/87ee2c3e-b17a-4939-b969-2c9c034d05d7?cache=false)](https://endimmfuncdev.azurewebsites.net/api/imm/github/ais-dotnet/Ais.Net/rule/87ee2c3e-b17a-4939-b969-2c9c034d05d7?cache=false)

[![Packaging](https://endimmfuncdev.azurewebsites.net/api/imm/github/ais-dotnet/Ais.Net/rule/547fd9f5-9caf-449f-82d9-4fba9e7ce13a?cache=false)](https://endimmfuncdev.azurewebsites.net/api/imm/github/ais-dotnet/Ais.Net/rule/547fd9f5-9caf-449f-82d9-4fba9e7ce13a?cache=false)
