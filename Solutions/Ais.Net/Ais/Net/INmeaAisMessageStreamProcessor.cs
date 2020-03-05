// <copyright file="INmeaAisMessageStreamProcessor.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Ais.Net
{
    using System;

    /// <summary>
    /// Receives AIS messages parsed from  an NMEA file by <see cref="NmeaStreamParser"/>.
    /// </summary>
    /// <remarks>
    /// In cases where AIS messages have been fragmented across multiple lines, they will be
    /// reassembled and passed to implementors of this interface as single messages.
    /// </remarks>
    public interface INmeaAisMessageStreamProcessor
    {
        /// <summary>
        /// Called for each complete AIS message.
        /// </summary>
        /// <param name="firstLine">The parsed line.</param>
        /// <param name="asciiPayload">The full payload.</param>
        /// <param name="padding">The number of bits of padding at the end of the payload.</param>
        /// <remarks>
        /// Where the source message was not fragmented, the <c>asciiPayload</c> will refer
        /// directly to the original message's payload in situ. If the AIS message was split
        /// across multiple NMEA sentences, the various sections of the payload will have been
        /// copied into a reassembly buffer, so that it can be presented as a single contiguous
        /// block of data.
        /// </remarks>
        void OnNext(
            in NmeaLineParser firstLine,
            in ReadOnlySpan<byte> asciiPayload,
            uint padding);

        /// <summary>
        /// Called when a line cannot be parsed, e.g. it does not contain a well-formed NMEA
        /// message.
        /// </summary>
        /// <param name="line">The line that cannot be parsed.</param>
        /// <param name="error">An exception describing the problem.</param>
        /// <param name="lineNumber">The 1-based line number on which the error was detected.</param>
        void OnError(in ReadOnlySpan<byte> line, Exception error, int lineNumber);

        /// <summary>
        /// Called when all lines have been processed.
        /// </summary>
        void OnCompleted();

        /// <summary>
        /// Called regularly to provide progress reports, and always called at the end.
        /// </summary>
        /// <param name="done">True if all processing is complete.</param>
        /// <param name="totalNmeaLines">The number of NMEA lines processed.</param>
        /// <param name="totalAisMessages">The number of AIS messages processed.</param>
        /// <param name="totalTicks">The number of milliseconds of processing.</param>
        /// <param name="nmeaLinesSinceLastUpdate">The number of lines since the last progress notification.</param>
        /// <param name="aisMessagesSinceLastUpdate">The number of AIS messages since the last progress notification.</param>
        /// <param name="ticksSinceLastUpdate">The number of ticks since the last progress notification.</param>
        void Progress(
            bool done,
            int totalNmeaLines,
            int totalAisMessages,
            int totalTicks,
            int nmeaLinesSinceLastUpdate,
            int aisMessagesSinceLastUpdate,
            int ticksSinceLastUpdate);
    }
}