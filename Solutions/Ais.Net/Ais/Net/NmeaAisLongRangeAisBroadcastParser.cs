// <copyright file="NmeaAisLongRangeAisBroadcastParser.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Ais.Net
{
    using System;

    /// <summary>
    /// Enables fields to be extracted from a Long Range AIS Broadcast (Message Type 27) payload in
    /// an NMEA sentence.
    /// </summary>
    public readonly ref struct NmeaAisLongRangeAisBroadcastParser
    {
        private readonly NmeaAisBitVectorParser bits;

        /// <summary>
        /// Create an <see cref="NmeaAisPositionReportClassAParser"/>.
        /// </summary>
        /// <param name="ascii">The ASCII-encoded message payload.</param>
        /// <param name="padding">The number of bits of padding in this payload.</param>
        public NmeaAisLongRangeAisBroadcastParser(ReadOnlySpan<byte> ascii, uint padding)
        {
            this.bits = new NmeaAisBitVectorParser(ascii, padding);
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
        /// Gets a value indicating whether the position information is of DGPS quality.
        /// </summary>
        /// <remarks>
        /// If <c>true</c>, location information is DGPS-quality (less than 10m). If false, it is
        /// of unaugmented GNSS accuracy.
        /// </remarks>
        public bool PositionAccuracy => this.bits.GetBit(38);

        /// <summary>
        /// Gets a value indicating whether Receiver Autonomous Integrity Monitoring is in use.
        /// </summary>
        public bool RaimFlag => this.bits.GetBit(39);

        /// <summary>
        /// Gets the vessel's navigation status.
        /// </summary>
        public NavigationStatus NavigationStatus => (NavigationStatus)this.bits.GetUnsignedInteger(4, 40);

        /// <summary>
        /// Gets the reported longitude, in units of 1/10 arc minutes.
        /// </summary>
        public int Longitude10thMins => this.bits.GetSignedInteger(18, 44);

        /// <summary>
        /// Gets the reported latitude, in units of 1/10 arc minutes.
        /// </summary>
        public int Latitude10thMins => this.bits.GetSignedInteger(17, 62);

        /// <summary>
        /// Gets the vessel's speed over ground, in tenths of a knot.
        /// </summary>
        public uint SpeedOverGroundTenths => this.bits.GetUnsignedInteger(6, 79);

        /// <summary>
        /// Gets the vessel's course over ground in degrees.
        /// </summary>
        public uint CourseOverGroundDegrees => this.bits.GetUnsignedInteger(9, 85);

        /// <summary>
        /// Gets a value indicating whether position is GNSS. False if this is "current GNSS position", true if "not GNSS position".
        /// </summary>
        public bool NotGnssPosition => this.bits.GetBit(94);

        /// <summary>
        /// Gets a value indicating whether the spare bit at offset 95 is set.
        /// </summary>
        public bool Spare95 => this.bits.GetBit(95);
    }
}
