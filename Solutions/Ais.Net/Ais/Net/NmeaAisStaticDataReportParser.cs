// <copyright file="NmeaAisStaticDataReportParser.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Ais.Net
{
    using System;

    /// <summary>
    /// Performs initial parsing of type 24 messages to distinguish between Part A and Part B messages.
    /// </summary>
    public static class NmeaAisStaticDataReportParser
    {
        /// <summary>
        /// Inspects a Type 24: Static Data Report message and determines the message Part Number.
        /// </summary>
        /// <param name="ascii">The ASCII-encoded message payload.</param>
        /// <param name="padding">The number of bits of padding in this payload.</param>
        /// <returns>The part number.</returns>
        public static uint GetPartNumber(ReadOnlySpan<byte> ascii, uint padding)
        {
            var bits = new NmeaAisBitVectorParser(ascii, padding);
            return bits.GetUnsignedInteger(2, 38);
        }
    }
}
