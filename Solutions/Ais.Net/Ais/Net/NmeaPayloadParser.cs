// <copyright file="NmeaPayloadParser.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Ais.Net
{
    using System;

    /// <summary>
    /// Processing for the AIS payload of an NMEA line.
    /// </summary>
    public readonly ref struct NmeaPayloadParser
    {
        /// <summary>
        /// Discover the message type of a payload prior to full parsing.
        /// </summary>
        /// <param name="data">The payload data.</param>
        /// <param name="padding">
        /// The number of padding bits reported in the AIDVM/AIVDO sentence.
        /// </param>
        /// <returns>The message type.</returns>
        public static int PeekMessageType(ReadOnlySpan<byte> data, uint padding)
        {
            if (data.Length == 0 || (data.Length == 1 && padding != 0))
            {
                throw new ArgumentException("There must be at least 6 bits of data to determine the message type");
            }

            return AisAsciiTo6Bits(data[0]);
        }

        /// <summary>
        /// Convert an ASCII-endcoded byte from the AIVDM/AIVDO 6-bit encoded format into its numeric value.
        /// </summary>
        /// <param name="c">The character.</param>
        /// <returns>The value signified by the character.</returns>
        internal static byte AisAsciiTo6Bits(byte c) => (byte)(c < 48
            ? throw new ArgumentOutOfRangeException("Payload characters must be in range 48-87 or 96-119")
            : (c < 88
                ? c - 48
                : (c < 96
                    ? throw new ArgumentOutOfRangeException("Payload characters must be in range 48-87 or 96-119")
                    : (c < 120
                        ? c - 56
                        : throw new ArgumentOutOfRangeException("Payload characters must be in range 48-87 or 96-119")))));
    }
}