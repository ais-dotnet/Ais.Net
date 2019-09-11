// <copyright file="NmeaTagBlockSentenceGrouping.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Ais.Net
{
    /// <summary>
    /// Sentence grouping information from an NMEA tag block.
    /// </summary>
    /// <remarks>
    /// This is present on messages that have been fragmented.
    /// </remarks>
    public readonly struct NmeaTagBlockSentenceGrouping
    {
        /// <summary>
        /// Creates a <see cref="NmeaTagBlockSentenceGrouping"/>.
        /// </summary>
        /// <param name="sentenceNumber">The <see cref="SentenceNumber"/> property.</param>
        /// <param name="sentencesInGroup">The <see cref="SentencesInGroup"/> property.</param>
        /// <param name="groupId">The <see cref="GroupId"/> property.</param>
        public NmeaTagBlockSentenceGrouping(int sentenceNumber, int sentencesInGroup, int groupId)
        {
            this.SentenceNumber = sentenceNumber;
            this.SentencesInGroup = sentencesInGroup;
            this.GroupId = groupId;
        }

        /// <summary>
        /// Gets the sentence group id.
        /// </summary>
        public int GroupId { get; }

        /// <summary>
        /// Gets the 1-based number of this sentence within the group.
        /// </summary>
        public int SentenceNumber { get; }

        /// <summary>
        /// Gets the total number of sentences in this group.
        /// </summary>
        public int SentencesInGroup { get; }
    }
}