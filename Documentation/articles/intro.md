# Introduction to Ais.Net

Ais.Net is a high-performance parser for NMEA/AIS messages. It uses a low-allocation design to
enable efficient, high-throughput processing of NMEA sentences containing AIS messages.

## Basic usage

The normal way to process messages with this library is to implement the
[INmeaAisMessageStreamProcessor](https://ais-dotnet.github.io/Ais.Net/api/Ais.Net.INmeaAisMessageStreamProcessor.html)
interface, and then call one of the methods provided by
[NmeaStreamParser](https://ais-dotnet.github.io/Ais.Net/api/Ais.Net.NmeaStreamParser.html).
For example, if the data you want to process is in a file, you can call `NmeaStreamParser.ParseFileAsync`. The
`ReadAllPositions` class in the `Ais.Net.Benchmarks` project does this:

```csharp
public static async Task ProcessMessagesFromFile(string path)
{
    await NmeaStreamParser.ParseFileAsync(path, Processor).ConfigureAwait(false);
}
```

Your `INmeaAisMessageStreamProcessor` implementation must supply three methods. The `NmeaStreamParser` will call
your `OnNext` method once for each complete AIS message (which might span multiple NMEA sentences; the limited length
of NMEA sentences means that long AIS messages get fragmented). It calls `OnCompleted` after all messages have been
processed. And from time to time it will call `Progress`. (It calls this once every 100,000 messages, and also right
before `OnComplete`.) The most important of these is `OnNext`, which must have this signature:

```csharp
void OnNext(
    in NmeaLineParser firstLine,
    in ReadOnlySpan<byte> asciiPayload,
    uint padding);
```

**Note**: you must process the message completely before returning—`NmeaStreamParser` reuses buffers to minimize,
allocations, so it and guarantees only that the data passed to `OnNext` remains available until `OnNext` returns. See
the [Low-allocation design](#low-allocation-design) section later for more details

The `OnNext` method's arguments may seem a little odd at first, because `NmeaLineParser` defines `Payload` and
`Padding` properties, and in many cases these will report the same value as the 2nd and 3rd arguments. But in cases
where a single AIS message is split across multiple NMEA sentences, the `firstLine` argument will provide only the
first sentence, whereas the `asciiPayload` and `padding` will provide the entire payload, assembled from multiple
individual sentences if necessary. You should therefore always use the 2nd and 3rd arguments to access the payload;
the `firstLine` is provided so that you can get hold of the other data from the NMEA sentence.

**Note**: if you implement the `INmeaAisMessageStreamProcessor` interface, you will have access to the entire payload
of any split messages, but there are two types of information you will not receive. First, if a split message is
incomplete, the `NmeaStreamParser` will not pass it to your `INmeaAisMessageStreamProcessor`, so you will never see
message fragments. Second, in cases where all fragments are present, you will not have access to the NMEA fields other
than the payload for sentences other than the first fragment. In most cases this is fine because the only fields in
the other fragments that are different are the ones used for indicating which fragment they are. However, if you really
need to see everything, you can instead implement `INmeaLineStreamProcessor`. This passes you every single NMEA line,
and makes no attempt to reassemble messages fragmented across multiple sentences.

You will receive the payload as a `ReadOnySpan<byte>`. (That's true whichever of the two interfaces you implement–even
if you use the lower-level `INmeaLineStreamProcessor`, the payloads are available through the `NmeaLineParser.Payload`
property, which is of type `ReadOnlySpan<byte>`.) The first thing you'll typically want to do with this is discover
which type of message you've got. You can do this with the `NmeaPayloadParser.PeekMessageType` method.

```csharp
int messageType = NmeaPayloadParser.PeekMessageType(asciiPayload, padding);
```

This will determine how (or whether) you will go on to process the payload. Ais.Net provides various types for
parsing different AIS message formats. Currently, 6 parsers are supplied, covering 8 message types:


| Message type(s)   | Name                                |
|-------------------|-------------------------------------|
| 1, 2, 3           | Position Report Class A             |
| 5                 | Static Voyage Related Data          |
| 18                | Standard Class B CS Position Report |
| 19                | Extended Class B CS Position Report |
| 24                | Static Data Report                  |
| 27                | Long Range AIS Broadcast message    |

To use a parser, you construct it with the `ReadOnlySpan<byte>` for the payload, e.g.:

```csharp
if (messageType >= 1 && messageType <= 3)
{
    var parser = new NmeaAisPositionReportClassAParser(asciiPayload, padding);
    Console.WriteLine($"Position: {parser.Latitude10000thMins / 60000.0},{parser.Longitude10000thMins / 60000.0}");
}
```

Each of the parser types defines a property for each field in the corresponding AIS message type. For the most part
these are pretty straightforward. The one potentially surprising feature is that none of the text fields returns a
`string`. That's because the goal of this library is to minimize allocations, and to return a `string` it is usually
necessary to allocate space for it on the GC heap. (The only exception is if the string can have one of a small set of
known values, in which case you can return the same one every time for any particular value, instead of necessarily
allocating a new one every time.) Text is instead represented using `NmeaAisTextFieldParser`.

`NmeaAisTextFieldParser` lets you retrieve individual characters by index, or you can ask it to write the entire string
in ASCII form into a `Span<byte>`. If you really want it as a `string` you can always do this:

```csharp
var parser = new NmeaAisStaticAndVoyageRelatedDataParser(
    lineParser.Payload, lineParser.Padding);
Span<byte> vesselNameAscii = stackalloc byte[(int)parser.VesselName.CharacterCount];
parser.VesselName.WriteAsAscii(vesselNameAscii);
// CAUTION: this will cause an allocation. Don't do this unless you have to
string vesselName = Encoding.ASCII.GetString(vesselNameAscii);
```

By the way, the reason for copying the data into a separate `Span<byte>` first is that it is not stored in ASCII
format in the AIS message. AIS uses its own 6-bit character encoding. The `NmeaAisTextFieldParser.WriteAsAscii`
method converts from this into ASCII as it copies the data out.


## Low-allocation design

Ais.Net is designed to minimize GC overhead. As with any library there is some initial memory
overhead to pay simply to use the code, but once you are up and running it is possible to
process messages with no per-message allocations.

The benchmarks built into the library source repository include a test that extracts location
information from a file containing 1 million messages, and this has exactly the same memory
allocation characteristics as a test that reads 1,000, or 10 million messages, demonstrating
that after the first use, we make zero allocations per-message.

Ais.Net achieves this by taking advantage of `Span<T>` and related types which were added in
.NET Core 2.1, and using associated language features added in C# 7.2. This makes Ais.Net
somewhat different to use from other .NET AIS parsers.

The `Span<T>` and `ReadOnlySpan<T>` types impose a constraint in exchange for the high
performance they make possible: these types can only live on the stack. The C# compiler
knows this because these types are declared as `ref struct`, and it will prevent you
from using these types in ways that could end up on the heap. For example, if you try to
define a `class` with a field of type `ReadOnlySpan<T>`, you get a compiler error, because
instances of classes live on the heap. You'll get the same error with an ordinary `struct`, because
although those often live on the stack, they can also live on the heap. (They might be
boxed, or they could be an element in an array, or a field in some other heap-based instance.)
You can use a `Span<T>` or `ReadOnlySpan<T>` (or any other `ref struct`) as a field only in
another `ref struct`. Since the various payload parsers that Ais.Net defines, such as
[NmeaAisPositionReportClassAParser](https://ais-dotnet.github.io/Ais.Net/api/Ais.Net.NmeaAisPositionReportClassAParser.html),
have `ReadOnlySpan<T>` fields, these parser types are all defined as `ref struct`,
meaning that they can only live on the stack. So you can do this:

```csharp
public static (double Latitude, double Longitude) GetPosition(byte[] asciiNmeaLine)
{
    var parser = new NmeaAisPositionReportClassAParser(asciiNmeaLine);
    return (parser.Latitude10000thMins / 600000, parser.Longitude10000thMins / 600000);
}
```

But this will not work:

```csharp
public class PositionExtractor
{
    // This will produce a compiler error, because you cannot have a ref struct
    // a field in a non-ref-struct type.
    private readonly NmeaAisPositionReportClassAParser parser;
    public PositionExtractor(byte[] asciiNmeaLine)
    {
        this.parser = new NmeaAisPositionReportClassAParser(asciiNmeaLine);
    }
}
```

More subtly, this restriction prevents you from using `ref struct` types as local variables
in `async` methods. To enable `async` methods to continue after an `await` completes
asynchronously, C# needs to be able to store all local variables on the heap, which means
those variables must not include any `ref struct` types. This means you cannot use the various
Ais.Net `Parser` types as variables in such methods. If you need to parse an NMEA message from
an asynchronous method, you can use a nested method, e.g.:

```csharp
public static async Task<(double latitude, double longitude)> GetPositionAsync()
{
    ReadOnlyMemory<byte> line = await GetLineAsync();
    return ParseLine();

    (double latitude, double longitude) ParseLine()
    {
        var parser = new NmeaAisPositionReportClassAParser(line.Span, 0);
        return (parser.Latitude10000thMins / 60000.0, parser.Longitude10000thMins / 60000.0);
    }
}
```

The code inside the nested `ParseLine()` method here would not compiler if it were in the containing
`GetPositionAsync()` method, since that is `async`. But since the nested method itself is not, it is allowed to
have local `ref struct` variables.
