# Error handling strategy

## Status

Established. (The practices were in place by the time we started adding ADRs.)

## Context

NMEA messages containing AIS data are sometimes malformed. The messages are often transferred from transponder devices to computer equipment over a serial port, which have limited error detection and correction capabilities, making it possible for messages to lose characters or contain garbage characters.

## Decision

We deal with malformed NMEA messages at two levels.

The `NmeaLineParser` detects failures to conform to the basic structure required of an NMEA message. It reports these errors by throwing an `ArgumentException` with a text message describing the way in which the message structure does not match the specification. For example, if the normal `!` start character is missing, the error message is `Invalid data. Expected '!' at sentence start`. In cases where the message is well formed but with features that Ais.Net cannot currently parse, we throw a `NotSupportedException` exception.

Code that processes large numbers of messages will typically not construct the `NmeaLineParser` itself, and instead relies on the high-throughput methods offered by `NmeaStreamParser`. Code working this way passes in an implementation of either `INmeaLineStreamProcessor` or `INmeaAisMessageStreamProcessor` depending on whether it wants to process each line in the file directly, or it wants `NmeaStreamParser` to handle the task of reassembling AIS payloads that have been fragmented across multiple NMEA message lines. Both of these interfaces require an `OnError` method that looks like this:

```csharp
void OnError(in ReadOnlySpan<byte> line, Exception error, int lineNumber);
```

When the `NmeaLineParser` constructed by the `NmeaStreamParser` reports an error in the manner described above, the exception it throws will be passed to this `OnError` method, along with a span providing access to the problematic line's full content, and the line number within the stream at which the error was detected.

There are also errors that can occur with fragmented messages. While each individual fragment might be well-formed, they might be wrong in combination. For example, it is not permitted for a fragmented message to have padding at the end of the first fragment. In these cases, which will be detected when working at the message level, we report errors through the same mechanism.


## Consequences

The `NmeaLineParser`-level error reporting provides applications with a clear description of what seems to be wrong, helping to diagnose the problem. The stream-level handling makes it possible for high-volume processing code to continue in the face of errors in a data stream.