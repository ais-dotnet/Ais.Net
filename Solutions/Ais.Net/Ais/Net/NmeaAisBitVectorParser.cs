// <copyright file="NmeaAisBitVectorParser.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Ais.Net
{
    using System;

    /// <summary>
    /// Extracts values from a bit vector encoded using the NMEA 6-bit ASCII encoding for AIS
    /// payloads (AIVDM/AIVDO Payload Armoring).
    /// </summary>
    public readonly ref struct NmeaAisBitVectorParser
    {
        private readonly ReadOnlySpan<byte> ascii;

        /// <summary>
        /// Creates a <see cref="NmeaAisBitVectorParser"/>.
        /// </summary>
        /// <param name="ascii">The ASCII data to be decoded.</param>
        /// <param name="padding">
        /// The number of bits that are present at the end of the data that are to be ignored.
        /// (The ASCII encoding works in multiples of 6 bits, so we may end up with more bits
        /// present than were present in the source.)
        /// </param>
        public NmeaAisBitVectorParser(ReadOnlySpan<byte> ascii, uint padding)
        {
            this.ascii = ascii;
            this.BitCount = checked((uint)((ascii.Length * 6) - padding));
        }

        /// <summary>
        /// Gets the number of valid bits in the payload (excluding any padding).
        /// </summary>
        public uint BitCount { get; }

        /// <summary>
        /// Gets the value of a signed integer bitfield.
        /// </summary>
        /// <param name="bitCount">The number of bits in the value.</param>
        /// <param name="bitOffset">The offset (in bits) of the bitfield within the bit vector.</param>
        /// <returns>The value.</returns>
        public int GetSignedInteger(uint bitCount, uint bitOffset)
        {
            int result = (int)this.GetUnsignedInteger(bitCount, bitOffset);

            int sbitCount = (int)bitCount;
            int msb = 1 << (sbitCount - 1);
            bool isNegative = (result & msb) != 0;

            if (isNegative)
            {
                const int allOnesExceptLsb = -2;
                int signBits = allOnesExceptLsb << (sbitCount - 1);
                result |= signBits;
            }

            return result;
        }

        /// <summary>
        /// Gets the value of an unsigned integer bitfield.
        /// </summary>
        /// <param name="bitCount">The number of bits in the value. M.</param>
        /// <param name="bitOffset">The offset (in bits) of the bitfield within the bit vector.</param>
        /// <returns>The value.</returns>
        public uint GetUnsignedInteger(uint bitCount, uint bitOffset)
        {
            if (bitOffset + bitCount > this.BitCount)
            {
                throw new ArgumentOutOfRangeException("Would read off end of bit vector");
            }

            if (bitCount > 32)
            {
                throw new ArgumentOutOfRangeException("Cannot read fields larger than 32 bits");
            }

            uint result = 0;
            int charOffset = (int)(bitOffset / 6);

            // Using int for these (despite both being inherently non-negative values) because
            // C#'s << doesn't accept a uint as its 2nd argument!
            int currentBitOffset = (int)bitOffset % 6;
            int remainingBits = (int)bitCount;

            while (remainingBits > 0)
            {
                result <<= Math.Min(6, remainingBits);

                // uint may seem like a strange choice since these are all inherently byte-like,
                // but since the bitwise & operator doesn't actually support byte, C# ends up
                // promoting arguments to bitwise operations against bytes to int anyway. So
                // instead of fighting the compiler by adding (byte) casts everywhere (possibly
                // causing generation of unnecessary type conversion code) we may as well just work
                // with the natural word size.
                uint currentCharacterValue = NmeaPayloadParser.AisAsciiTo6Bits(this.ascii[charOffset]);
                uint valueDiscardingBitsBeforeBitOffset;

                if (currentBitOffset == 0)
                {
                    valueDiscardingBitsBeforeBitOffset = currentCharacterValue;
                }
                else
                {
                    // If we're looking at something of this form:
                    //   011010
                    // and the current bit offset is, say, 3, we want to ignore the first three
                    // bits, leaving just 010.
                    //  offset  mask
                    //  0       111111
                    //  1       011111
                    //  2       001111
                    // etc.
                    byte bitMask = (byte)(0b_111111 >> currentBitOffset);
                    valueDiscardingBitsBeforeBitOffset = currentCharacterValue & bitMask;
                }

                uint valueDiscardingTrailingBits;
                int bitsLeftAfterInitialDiscard = 6 - currentBitOffset;

                if (remainingBits >= bitsLeftAfterInitialDiscard)
                {
                    valueDiscardingTrailingBits = valueDiscardingBitsBeforeBitOffset;
                    remainingBits -= bitsLeftAfterInitialDiscard;
                }
                else
                {
                    valueDiscardingTrailingBits = valueDiscardingBitsBeforeBitOffset >> (bitsLeftAfterInitialDiscard - remainingBits);
                    remainingBits = 0;
                }

                result += valueDiscardingTrailingBits;
                charOffset += 1;
                currentBitOffset = 0;
            }

            return result;
        }

        /// <summary>
        /// Gets the value of the specified bit.
        /// </summary>
        /// <param name="bitOffset">The position of the bit to read.</param>
        /// <returns>True if the bit is 1, false if it is 0.</returns>
        public bool GetBit(uint bitOffset) => this.GetUnsignedInteger(1, bitOffset) == 1;
    }
}