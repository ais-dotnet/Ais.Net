// <copyright file="NmeaTagBlockParser.cs" company="Endjin Limited">
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

                // Adding scoped here keeps the compiler happy, but it's not clear what additional
                // powers this method would have had without the scoped keyword. It's not like
                // we're *not* making it escape: we return a span derived from source! It's possible
                // that the only effect here is that it changes the effective safe-to-escape of
                // the return value. Without the scoped, it might be that the return value would
                // be treated as more escapable?
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

        // As with AdvanceToNextField above, I can't see what limits scoped is imposing on this
        // method, but the compiler isn't happy unless we add it.
        private static bool GetEnd(scoped in ReadOnlySpan<byte> source, char? delimiter, out ReadOnlySpan<byte> remaining, out int length)
        {
            if (delimiter.HasValue)
            {
                length = source.IndexOf((byte)delimiter.Value);

                if (length < 0)
                {
                    remaining = source;
                    return false;
                }

                remaining = source.Slice(length + 1);
            }
            else
            {
                length = source.IndexOf((byte)',');
                bool isLastField = length < 0;

                if (isLastField)
                {
                    remaining = ReadOnlySpan<byte>.Empty;
                    length = source.Length;
                }
                else
                {
                    remaining = source.Slice(length + 1);
                }
            }

            return true;
        }

        private static bool ParseDelimitedInt(ref ReadOnlySpan<byte> source, out int result, char? delimiter = null)
        {
            result = default;

            if (!GetEnd(source, delimiter, out ReadOnlySpan<byte> remaining, out int length))
            {
                return false;
            }

            if (!Utf8Parser.TryParse(source, out result, out int consumed)
                || consumed != length)
            {
                return false;
            }

            source = remaining;

            return true;
        }

        private static bool ParseDelimitedLong(ref ReadOnlySpan<byte> source, out long result, char? delimiter = null)
        {
            result = default;

            if (!GetEnd(source, delimiter, out ReadOnlySpan<byte> remaining, out int length))
            {
                return false;
            }

            if (!Utf8Parser.TryParse(source, out result, out int consumed)
                || consumed != length)
            {
                return false;
            }

            source = remaining;

            return true;
        }

        // This needed to change to a static member under the C# 11.0 rules.
        // Why is that?
        // Rather surprisingly, it's because starting with C# 11.0, you can initialize spans with
        // references to local variables. But it's not remotely obvious why that means this method
        // became unacceptable.
        // The basic problem is that as an instance member, it is able to replace the caller's
        // ReadOnlySpan<byte> with one that it captured earlier, e.g., during construction.
        // Under the design of the ref safety rules in C# 7.2, this didn't present any problems,
        // so it was allowed. But the rules have changed in C# 11.0 to accommodate the ability to
        // create spans that refer to local variables, as described in
        //  https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/proposals/csharp-11.0/low-level-struct-improvements#compat-considerations
        // (And more generally, ref inputs can be captured as ref fields, although it appears that
        // the compiler currently imposes stricter constraints on this than it does for spans, so
        // at the moment, you need to use spans to run into the problem.) When it was impossible
        // for a span to refer to a local variable, there was no possibility for a ref argument
        // to be promoted to some ref struct that held onto that ref. But now that this is
        // possible, the compiler has to be more conservative about which calls it allows to
        // instance methods of ref structs which accept ref or out arguments of ref struct type.
        // It's still not 100% clear to me exactly how the rule changes have worked here, but I
        // think  it might be something to do with the "Method Arguments Must Match" rule,
        // which used to look like this:
        //  https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/proposals/csharp-7.2/span-safety#method-arguments-must-match
        // and has been changed thus:
        //  https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/proposals/csharp-11.0/low-level-struct-improvements#method-arguments-must-match
        // It's also possible that the "scoped" nature of "this" has had an impact. C# used to
        // consider the implicit 'this' of a ref struct to be a 'ref MyStructType', but in C# 11.0
        // this changed to 'scoped ref MyStructType', and it's possible that that's coming into
        // play here.
        // As far as I can tell, the fact that we are calling this from inside the constructor
        // also seems to have some impact.
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