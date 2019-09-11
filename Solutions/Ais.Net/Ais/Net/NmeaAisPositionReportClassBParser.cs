// <copyright file="NmeaAisPositionReportClassBParser.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Ais.Net
{
    using System;

    /// <summary>
    /// Enables fields to be extracted from an AIS Standard Class B CS Position Report payload in an NMEA
    /// sentence.
    /// </summary>
    public readonly ref struct NmeaAisPositionReportClassBParser
    {
        private readonly NmeaAisBitVectorParser bits;

        /// <summary>
        /// Create an <see cref="NmeaAisPositionReportClassBParser"/>.
        /// </summary>
        /// <param name="ascii">The ASCII-encoded message payload.</param>
        /// <param name="padding">The number of bits of padding in this payload.</param>
        public NmeaAisPositionReportClassBParser(ReadOnlySpan<byte> ascii, uint padding)
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
        /// Gets the 8 bits of 'regional reserved' data starting at bit 38.
        /// </summary>
        public byte RegionalReserved38 => (byte)this.bits.GetUnsignedInteger(8, 38);

        /// <summary>
        /// Gets the vessel's speed over ground, in tenths of a knot.
        /// </summary>
        public uint SpeedOverGroundTenths => this.bits.GetUnsignedInteger(10, 46);

        /// <summary>
        /// Gets a value indicating whether the position information is of DGPS quality.
        /// </summary>
        /// <remarks>
        /// If <c>true</c>, location information is DGPS-quality (less than 10m). If false, it is
        /// of unaugmented GNSS accuracy.
        /// </remarks>
        public bool PositionAccuracy => this.bits.GetBit(56);

        /// <summary>
        /// Gets the reported longitude, in units of 1/10000 arc minutes.
        /// </summary>
        public int Longitude10000thMins => this.bits.GetSignedInteger(28, 57);

        /// <summary>
        /// Gets the reported latitude, in units of 1/10000 arc minutes.
        /// </summary>
        public int Latitude10000thMins => this.bits.GetSignedInteger(27, 85);

        /// <summary>
        /// Gets the vessel's course over ground in units of one tenth of a degree.
        /// </summary>
        public uint CourseOverGround10thDegrees => this.bits.GetUnsignedInteger(12, 112);

        /// <summary>
        /// Gets the vessel's heading in degrees.
        /// </summary>
        public uint TrueHeadingDegrees => this.bits.GetUnsignedInteger(9, 124);

        /// <summary>
        /// Gets the seconds part of the (UTC) time at which the location was recorded.
        /// </summary>
        public uint TimeStampSecond => this.bits.GetUnsignedInteger(6, 133);

        /// <summary>
        /// Gets the 2 bits of 'regional reserved' data starting at bit 38.
        /// </summary>
        public byte RegionalReserved139 => (byte)this.bits.GetUnsignedInteger(2, 139);

        /// <summary>
        /// Gets the value indicating whether this is a Class B (using CSTDMA) unit, or a Class B+
        /// (using SOTDMA) unit.
        /// </summary>
        public ClassBUnit CsUnit => this.bits.GetBit(141) ? ClassBUnit.Cstdma : ClassBUnit.Sotdma;

        /// <summary>
        /// Gets a value indicating whether the unit has a visual display attached.
        /// </summary>
        public bool HasDisplay => this.bits.GetBit(142);

        /// <summary>
        /// Gets a value indicating whether the unit is attached to a VHF voice radio with DSC
        /// capability.
        /// </summary>
        public bool IsDscAttached => this.bits.GetBit(143);

        /// <summary>
        /// Gets a value indicating whether base stations can command this unit to switch
        /// frequency.
        /// </summary>
        public bool CanSwitchBands => this.bits.GetBit(144);

        /// <summary>
        /// Gets a value indicating whether the unit can accept channel assignments via Message
        /// Type 22.
        /// </summary>
        public bool CanAcceptMessage22ChannelAssignment => this.bits.GetBit(145);

        /// <summary>
        /// Gets a value indicating whether the unit is running in assigned mode. If false, the
        /// unit is in autonomous mode.
        /// </summary>
        public bool IsAssigned => this.bits.GetBit(146);

        /// <summary>
        /// Gets a value indicating whether Receiver Autonomous Integrity Monitoring is in use.
        /// </summary>
        public bool RaimFlag => this.bits.GetBit(147);

        /// <summary>
        /// Gets a value indicating whether the radio status is in SOTDMA or ITDMA format.
        /// </summary>
        public ClassBRadioStatusType RadioStatusType => this.bits.GetBit(148)
            ? ClassBRadioStatusType.Itdma : ClassBRadioStatusType.Sotdma;
    }
}