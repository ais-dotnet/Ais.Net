// <copyright file="INmeaLineStreamProcessor.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Ais.Net
{
    using System;

    /// <summary>
    /// Receives the lines parsed from an NMEA file by <see cref="NmeaStreamParser"/>.
    /// </summary>
    /// <remarks>
    /// Processors implementing this interface will receive each separate line, which might contain
    /// only a fragment of an AIS message. If you want to process entire AIS messages, implement
    /// <see cref="INmeaAisMessageStreamProcessor"/> instead.
    /// </remarks>
    public interface INmeaLineStreamProcessor
    {
        /// <summary>
        /// Called for each non-empty line.
        /// </summary>
        /// <param name="parsedLine">The parsed line.</param>
        /// <param name="lineNumber">The 1-based line number.</param>
        void OnNext(in NmeaLineParser parsedLine, int lineNumber);

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
        /// <param name="totalLines">The number of lines processed.</param>
        /// <param name="totalTicks">The number of milliseconds of processing.</param>
        /// <param name="linesSinceLastUpdate">The number of lines since the last progress notification.</param>
        /// <param name="ticksSinceLastUpdate">The number of ticks since the last progress notification.</param>
        void Progress(
            bool done,
            int totalLines,
            int totalTicks,
            int linesSinceLastUpdate,
            int ticksSinceLastUpdate);
    }
}