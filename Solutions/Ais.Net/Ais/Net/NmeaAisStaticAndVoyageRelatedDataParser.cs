// <copyright file="NmeaAisStaticAndVoyageRelatedDataParser.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Ais.Net
{
    using System;

    /// <summary>
    /// Enables fields to be extracted from an AIS Static and Voyage Related Data payload in an
    /// NMEA sentence.
    /// </summary>
    public readonly ref struct NmeaAisStaticAndVoyageRelatedDataParser
    {
        private readonly NmeaAisBitVectorParser bits;

        /// <summary>
        /// Create an <see cref="NmeaAisStaticAndVoyageRelatedDataParser"/>.
        /// </summary>
        /// <param name="ascii">The ASCII-encoded message payload.</param>
        /// <param name="padding">The number of bits of padding in this payload.</param>
        public NmeaAisStaticAndVoyageRelatedDataParser(ReadOnlySpan<byte> ascii, uint padding)
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
        /// Gets the AIS Version field at bit offset 38.
        /// </summary>
        /// <remarks>
        /// According to the docs, 0 meants ITU1371 and 1-3 are reserved for future editions, but
        /// in practice we see all 4 possible values here.
        /// </remarks>
        public uint AisVersion => this.bits.GetUnsignedInteger(2, 38);

        /// <summary>
        /// Gets the IMO Ship ID number.
        /// </summary>
        public uint ImoNumber => this.bits.GetUnsignedInteger(30, 40);

        /// <summary>
        /// Gets the Call Sign field.
        /// </summary>
        public NmeaAisTextFieldParser CallSign => new NmeaAisTextFieldParser(this.bits, 42, 70);

        /// <summary>
        /// Gets the Call Sign field.
        /// </summary>
        public NmeaAisTextFieldParser VesselName => new NmeaAisTextFieldParser(this.bits, 120, 112);

        /// <summary>
        /// Gets the ship and cargo type.
        /// </summary>
        public ShipType ShipType => (ShipType)this.bits.GetUnsignedInteger(8, 232);

        /// <summary>
        /// Gets the distance in metres from the unit to the bow.
        /// </summary>
        public uint DimensionToBow => this.bits.GetUnsignedInteger(9, 240);

        /// <summary>
        /// Gets the distance in metres from the unit to the stern.
        /// </summary>
        public uint DimensionToStern => this.bits.GetUnsignedInteger(9, 249);

        /// <summary>
        /// Gets the distance in metres from the unit to port.
        /// </summary>
        public uint DimensionToPort => this.bits.GetUnsignedInteger(6, 258);

        /// <summary>
        /// Gets the distance in metres from the unit to starboard.
        /// </summary>
        public uint DimensionToStarboard => this.bits.GetUnsignedInteger(6, 264);

        /// <summary>
        /// Gets the position fix type.
        /// </summary>
        public EpfdFixType PositionFixType => (EpfdFixType)this.bits.GetUnsignedInteger(4, 270);

        /// <summary>
        /// Gets the month of the estimated time to arrival, or 0 if not available.
        /// </summary>
        public uint EtaMonth => this.bits.GetUnsignedInteger(4, 274);

        /// <summary>
        /// Gets the day of the estimated time to arrival, or 0 if not available.
        /// </summary>
        public uint EtaDay => this.bits.GetUnsignedInteger(5, 278);

        /// <summary>
        /// Gets the hour of the estimated time to arrival, or 0 if not available.
        /// </summary>
        public uint EtaHour => this.bits.GetUnsignedInteger(5, 283);

        /// <summary>
        /// Gets the minute of the estimated time to arrival, or 0 if not available.
        /// </summary>
        public uint EtaMinute => this.bits.GetUnsignedInteger(6, 288);

        /// <summary>
        /// Gets the vessel's draught in tenths of a metre.
        /// </summary>
        public uint Draught10thMetres => this.bits.GetUnsignedInteger(8, 294);

        /// <summary>
        /// Gets the Destination field.
        /// </summary>
        public NmeaAisTextFieldParser Destination => new NmeaAisTextFieldParser(this.bits, 120, 302);

        /// <summary>
        /// Gets a value indicating whether the data terminal is in a not ready state.
        /// </summary>
        public bool IsDteNotReady => this.bits.GetBit(422);

        /// <summary>
        /// Gets the value of the 'spare' bit at 423.
        /// </summary>
        public uint Spare423 => this.bits.GetUnsignedInteger(1, 423);
    }
}