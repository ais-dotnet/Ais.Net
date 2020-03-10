// <copyright file="NmeaLineParser.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Ais.Net
{
    using System;
    using System.Text;

    /// <summary>
    /// Parses a line of ASCII-encoded text containing an NMEA message.
    /// </summary>
    public readonly ref struct NmeaLineParser
    {
        private const byte ExclamationMark = (byte)'!';
        private const byte TagBlockMarker = (byte)'\\';
        private static readonly byte[] VdmAscii = Encoding.ASCII.GetBytes("VDM");
        private static readonly byte[] VdoAscii = Encoding.ASCII.GetBytes("VDO");
        private readonly bool throwWhenTagBlockContainsUnknownFields;

        /// <summary>
        /// Creates a <see cref="NmeaLineParser"/>.
        /// </summary>
        /// <param name="line">The ASCII-encoded text containing the NMEA message.</param>
        public NmeaLineParser(ReadOnlySpan<byte> line)
            : this(line, false)
        {
        }

        /// <summary>
        /// Creates a <see cref="NmeaLineParser"/>.
        /// </summary>
        /// <param name="line">The ASCII-encoded text containing the NMEA message.</param>
        /// <param name="throwWhenTagBlockContainsUnknownFields">
        /// Ignore non-standard and unsupported tag block field types. Useful when working with
        /// data sources that add non-standard fields.
        /// </param>
        public NmeaLineParser(ReadOnlySpan<byte> line, bool throwWhenTagBlockContainsUnknownFields)
        {
            this.Line = line;

            int sentenceStartIndex = 0;

            if (line[0] == TagBlockMarker)
            {
                int tagBlockEndIndex = line.Slice(1).IndexOf(TagBlockMarker);

                if (tagBlockEndIndex < 0)
                {
                    throw new ArgumentException("Unclosed tag block");
                }

                this.TagBlockAsciiWithoutDelimiters = line.Slice(1, tagBlockEndIndex);

                sentenceStartIndex = tagBlockEndIndex + 2;
            }
            else
            {
                if (line.IndexOf(TagBlockMarker) > 0)
                {
                    throw new NotSupportedException("Can't handle tag block unless at start");
                }

                this.TagBlockAsciiWithoutDelimiters = ReadOnlySpan<byte>.Empty;
            }

            this.Sentence = line.Slice(sentenceStartIndex);

            // Need at least the exclamation mark, talker, and origin (e.g. !AIVDM), then
            // the two fragment fields are non-optional. The multi-sequence message ID is
            // optional for non-fragmented messages, and apparently so is the channel.
            // so prior to the payload, we've got at least this much:
            // !AIVDM,1,1,,,
            // Then there will need to be the final comma, the padding, the * and the checksum, e.g.
            // ,0*3C
            // So that's 18 characters before we get to any payload.
            if (this.Sentence.Length < 18)
            {
                throw new ArgumentException("Invalid data. The message appears to have been truncated.");
            }

            if (this.Sentence[0] != ExclamationMark)
            {
                throw new ArgumentException("Invalid data. Expected '!' at sentence start");
            }

            byte talkerFirstChar = this.Sentence[1];
            byte talkerSecondChar = this.Sentence[2];

            this.AisTalker = talkerFirstChar switch
            {
                (byte)'A' => talkerSecondChar switch
                {
                    (byte)'I' => TalkerId.MobileStation,
                    (byte)'B' => TalkerId.BaseStation,
                    (byte)'D' => TalkerId.DependentBaseStation,
                    (byte)'N' => TalkerId.AidToNavigationStation,
                    (byte)'R' => TalkerId.ReceivingStation,
                    (byte)'S' => TalkerId.LimitedBaseStation,
                    (byte)'T' => TalkerId.TransmittingStation,
                    (byte)'X' => TalkerId.RepeaterStation,
                    _ => throw new ArgumentException("Unrecognized talker id - cannot start with " + talkerFirstChar),
                },

                (byte)'B' => talkerSecondChar switch
                {
                    (byte)'S' => TalkerId.DeprecatedBaseStation,
                    _ => throw new ArgumentException("Unrecognized talker id - cannot end with " + talkerSecondChar),
                },

                (byte)'S' => talkerSecondChar switch
                {
                    (byte)'A' => TalkerId.PhysicalShoreStation,
                    _ => throw new ArgumentException("Unrecognized talker id - cannot end with " + talkerSecondChar),
                },

                _ => throw new ArgumentException("Unrecognized talker id"),
            };
            if (this.Sentence.Slice(3, 3).SequenceEqual(VdmAscii))
            {
                this.DataOrigin = VesselDataOrigin.Vdm;
            }
            else if (this.Sentence.Slice(3, 3).SequenceEqual(VdoAscii))
            {
                this.DataOrigin = VesselDataOrigin.Vdo;
            }
            else
            {
                throw new ArgumentException("Unrecognized origin in AIS talker ID - must be VDM or VDO");
            }

            if (this.Sentence[6] != (byte)',')
            {
                throw new ArgumentException("Talker ID must be followed by ','");
            }

            ReadOnlySpan<byte> remainingFields = this.Sentence.Slice(7);

            this.TotalFragmentCount = GetSingleDigitField(ref remainingFields, true);
            this.FragmentNumberOneBased = GetSingleDigitField(ref remainingFields, true);

            int nextComma = remainingFields.IndexOf((byte)',');

            this.MultiSequenceMessageId = remainingFields.Slice(0, nextComma);

            remainingFields = remainingFields.Slice(nextComma + 1);
            nextComma = remainingFields.IndexOf((byte)',');

            if (nextComma > 1)
            {
                throw new ArgumentException("Channel code must be only one character");
            }

            this.ChannelCode = nextComma == 0 ? default : (char)remainingFields[0];

            remainingFields = remainingFields.Slice(nextComma + 1);
            nextComma = remainingFields.IndexOf((byte)',');

            this.Payload = remainingFields.Slice(0, nextComma);

            remainingFields = remainingFields.Slice(nextComma + 1);
            this.Padding = (uint)GetSingleDigitField(ref remainingFields, true);
            this.throwWhenTagBlockContainsUnknownFields = throwWhenTagBlockContainsUnknownFields;
        }

        /// <summary>
        /// Gets the talker ID that produced the message.
        /// </summary>
        public TalkerId AisTalker { get; }

        /// <summary>
        /// Gets the radio channel code, if present.
        /// </summary>
        public char? ChannelCode { get; }

        /// <summary>
        /// Gets the origin of the data (VDM or VDO).
        /// </summary>
        public VesselDataOrigin DataOrigin { get; }

        /// <summary>
        /// Gets the fragment number of this message, with 1 being the first fragment.
        /// </summary>
        public int FragmentNumberOneBased { get; }

        /// <summary>
        /// Gets the underlying data that was passed in during construction.
        /// </summary>
        public ReadOnlySpan<byte> Line { get; }

        /// <summary>
        /// Gets the multisequence message id if present (and an empty range if not).
        /// </summary>
        public ReadOnlySpan<byte> MultiSequenceMessageId { get; }

        /// <summary>
        /// Gets the number of bits of padding present in the payload.
        /// </summary>
        /// <remarks>
        /// The 6-bit ASCII encoding NMEA uses for the AIS payload can only encode data in
        /// multiples of 6 bits. If the underlying message is not a multiple of 6 bits long,
        /// the encoding will result in the payload having some extra bits on the end. This
        /// property reports how many of those bits there are.
        /// </remarks>
        public uint Padding { get; }

        /// <summary>
        /// Gets the 6-bit-ASCII-encoded payload. (This is the underlying AIS message.)
        /// </summary>
        public ReadOnlySpan<byte> Payload { get; }

        /// <summary>
        /// Gets the AIVDM/AIVDO sentence part of the underlying data.
        /// </summary>
        /// <remarks>
        /// In cases where no tag block is present, this will be the same as <see cref="Line"/>.
        /// But if a tag block was present, this provides just the 'sentence' part of the line.
        /// </remarks>
        public ReadOnlySpan<byte> Sentence { get; }

        /// <summary>
        /// Gets the details from the tag block.
        /// </summary>
        public NmeaTagBlockParser TagBlock => new NmeaTagBlockParser(this.TagBlockAsciiWithoutDelimiters, this.throwWhenTagBlockContainsUnknownFields);

        /// <summary>
        /// Gets the tag block part of the underlying data (excluding the delimiting '/'
        /// characters), or an empty span if no tag block was present.
        /// </summary>
        public ReadOnlySpan<byte> TagBlockAsciiWithoutDelimiters { get; }

        /// <summary>
        /// Gets the total number of message fragments in the set of messages of which this is a
        /// part.
        /// </summary>
        public int TotalFragmentCount { get; }

        private static int GetSingleDigitField(ref ReadOnlySpan<byte> fields, bool required)
        {
            if (fields[0] == ',')
            {
                if (required)
                {
                    throw new ArgumentException("Field must not be empty");
                }

                fields = fields.Slice(1);

                return 0;
            }

            int result = fields[0] - '0';

            if (result < 0 || result > 9)
            {
                throw new NotSupportedException("Cannot handle multi-digit field");
            }

            fields = fields.Slice(2);

            return result;
        }
    }
}