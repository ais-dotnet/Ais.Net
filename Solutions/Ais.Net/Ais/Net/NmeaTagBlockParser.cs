﻿// <copyright file="NmeaTagBlockParser.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Ais.Net
{
    using System;
    using System.Buffers.Text;

    /// <summary>
    /// Extracts data from the Tag Block section of an NMEA message.
    /// </summary>
    public readonly ref struct NmeaTagBlockParser
    {
        /// <summary>
        /// Creates a <see cref="NmeaTagBlockParser"/>.
        /// </summary>
        /// <param name="source">The ASCII-encoded tag block, without the leading and trailing
        /// <c>/</c> delimiters.
        /// </param>
        public NmeaTagBlockParser(ReadOnlySpan<byte> source)
            : this(source, false)
        {
        }

        /// <summary>
        /// Creates a <see cref="NmeaTagBlockParser"/>.
        /// </summary>
        /// <param name="source">The ASCII-encoded tag block, without the leading and trailing
        /// <c>/</c> delimiters.
        /// </param>
        /// <param name="throwWhenTagBlockContainsUnknownFields">
        /// Ignore non-standard and unsupported tag block field types. Useful when working with
        /// data sources that add non-standard fields.
        /// </param>
        public NmeaTagBlockParser(ReadOnlySpan<byte> source, bool throwWhenTagBlockContainsUnknownFields)
        {
            this.SentenceGrouping = default;
            this.Source = ReadOnlySpan<byte>.Empty;
            this.UnixTimestamp = default;

            if (source[source.Length - 3] != (byte)'*')
            {
                throw new ArgumentException("Tag blocks should end with *XX where XX is a two-digit hexadecimal checksum");
            }

            source = source.Slice(0, source.Length - 3);

            while (source.Length > 0)
            {
                char fieldType = (char)source[0];

                if (source.Length < 3 || source[1] != (byte)':')
                {
                    throw new ArgumentException("Tag block entries should start with a type character followed by a colon, and there was no colon");
                }

                source = source.Slice(2);

                switch (fieldType)
                {
                    case 'g':
                        this.SentenceGrouping = ParseSentenceGrouping(ref source);
                        break;

                    case 's':
                        this.Source = AdvanceToNextField(ref source);

                        break;

                    case 'c':
                        if (!ParseDelimitedLong(ref source, out long timestamp))
                        {
                            throw new ArgumentException("Tag block timestamp should be int");
                        }

                        this.UnixTimestamp = timestamp;
                        break;

                    case 'd':
                    case 'n':
                    case 'r':
                    case 't':
                        if (throwWhenTagBlockContainsUnknownFields)
                        {
                            throw new NotSupportedException("Unsupported field type: " + fieldType);
                        }
                        else
                        {
                            AdvanceToNextField(ref source);
                        }

                        break;

                    default:
                        if (throwWhenTagBlockContainsUnknownFields)
                        {
                            throw new ArgumentException("Unknown field type: " + fieldType);
                        }
                        else
                        {
                            AdvanceToNextField(ref source);
                        }

                        break;
                }

                static ReadOnlySpan<byte> AdvanceToNextField(scoped ref ReadOnlySpan<byte> source)
                {
                    ReadOnlySpan<byte> result;
                    int next = source.IndexOf((byte)',');
                    if (next < 0)
                    {
                        result = source;
                        source = ReadOnlySpan<byte>.Empty;
                    }
                    else
                    {
                        result = source.Slice(0, next);
                        source = source.Slice(next + 1);
                    }

                    return result;
                }
            }
        }

        /// <summary>
        /// Gets the sentence grouping information for fragmented messages, if present, null otherwise.
        /// </summary>
        public NmeaTagBlockSentenceGrouping? SentenceGrouping { get; }

        /// <summary>
        /// Gets the underlying data that was passed at construction.
        /// </summary>
        public ReadOnlySpan<byte> Source { get; }

        /// <summary>
        /// Gets the unix timestamp, if present, null otherwise.
        /// </summary>
        public long? UnixTimestamp { get; }

        private static bool GetEnd(ref ReadOnlySpan<byte> source, char? delimiter, out int length)
        {
            if (delimiter.HasValue)
            {
                length = source.IndexOf((byte)delimiter.Value);

                if (length < 0)
                {
                    return false;
                }

                source = source.Slice(length + 1);
            }
            else
            {
                length = source.IndexOf((byte)',');
                bool isLastField = length < 0;

                if (isLastField)
                {
                    length = source.Length;
                    source = ReadOnlySpan<byte>.Empty;
                }
                else
                {
                    source = source.Slice(length + 1);
                }
            }

            return true;
        }

        private static bool ParseDelimitedInt(ref ReadOnlySpan<byte> source, out int result, char? delimiter = null)
        {
            result = default;

            ReadOnlySpan<byte> original = source;
            if (!GetEnd(ref source, delimiter, out int length))
            {
                return false;
            }

            if (!Utf8Parser.TryParse(original, out result, out int consumed)
                || consumed != length)
            {
                return false;
            }

            return true;
        }

        private static bool ParseDelimitedLong(ref ReadOnlySpan<byte> source, out long result, char? delimiter = null)
        {
            result = default;

            ReadOnlySpan<byte> original = source;
            if (!GetEnd(ref source, delimiter, out int length))
            {
                return false;
            }

            if (!Utf8Parser.TryParse(original, out result, out int consumed)
                || consumed != length)
            {
                return false;
            }

            return true;
        }

        private static NmeaTagBlockSentenceGrouping ParseSentenceGrouping(ref ReadOnlySpan<byte> source)
        {
            if (!ParseDelimitedInt(ref source, out int sentenceNumber, '-'))
            {
                throw new ArgumentException("Tag block sentence grouping should be <int>-<int>-<int>, but first part was not a decimal integer");
            }

            if (!ParseDelimitedInt(ref source, out int totalSentences, '-'))
            {
                throw new ArgumentException("Tag block sentence grouping should be <int>-<int>-<int>, but second part was not a decimal integer");
            }

            if (!ParseDelimitedInt(ref source, out int groupId))
            {
                throw new ArgumentException("Tag block sentence grouping should be <int>-<int>-<int>, but third part was not a decimal integer");
            }

            return new NmeaTagBlockSentenceGrouping(sentenceNumber, totalSentences, groupId);
        }
    }
}