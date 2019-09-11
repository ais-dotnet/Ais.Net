// <copyright file="NmeaAisStaticDataReportParserPartA.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Ais.Net
{
    using System;

    /// <summary>
    /// Enables fields to be extracted from an AIS Static Data Report Part A payload in an
    /// NMEA sentence.
    /// </summary>
    public readonly ref struct NmeaAisStaticDataReportParserPartA
    {
        private readonly NmeaAisBitVectorParser bits;

        /// <summary>
        /// Create an <see cref="NmeaAisStaticDataReportParserPartA"/>.
        /// </summary>
        /// <param name="ascii">The ASCII-encoded message payload.</param>
        /// <param name="padding">The number of bits of padding in this payload.</param>
        public NmeaAisStaticDataReportParserPartA(ReadOnlySpan<byte> ascii, uint padding)
        {
            this.bits = new NmeaAisBitVectorParser(ascii, padding);
            if (this.PartNumber != 0)
            {
                throw new ArgumentException($"This is a parser for Part A (0) messages, but the part number of the message supplied is {this.PartNumber}");
            }
        }

        /// <summary>
        /// Gets the message type.
        /// </summary>
        public uint MessageType => this.bits.GetUnsignedInteger(6, 0);

        /// <summary>
        /// Gets the number of times this message had been repeated on this broadcast.
        /// </summary>
        /// <remarks>
        /// When stations retransmit messages with a view to enabling them to get around hills and
        /// other obstacles, this should be incremented. When it reaches 3, no more attempts should
        /// be made to retransmit it.
        /// </remarks>
        public uint RepeatIndicator => this.bits.GetUnsignedInteger(2, 6);

        /// <summary>
        /// Gets the unique identifier assigned to the transponder that sent this message.
        /// </summary>
        public uint Mmsi => this.bits.GetUnsignedInteger(30, 8);

        /// <summary>
        /// Gets the Part Number field.
        /// </summary>
        public uint PartNumber => this.bits.GetUnsignedInteger(2, 38);

        /// <summary>
        /// Gets the Vessel Name field.
        /// </summary>
        public NmeaAisTextFieldParser VesselName => new NmeaAisTextFieldParser(this.bits, 120, 40);

        /// <summary>
        /// Gets the value of the 'spare' bits at 160.
        /// </summary>
        public uint Spare160 => this.bits.BitCount == 168 ? this.bits.GetUnsignedInteger(8, 160) : 0;
    }
}
