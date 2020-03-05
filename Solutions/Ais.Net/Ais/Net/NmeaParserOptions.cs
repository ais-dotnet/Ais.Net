// <copyright file="NmeaParserOptions.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Ais.Net
{
    /// <summary>
    /// Enables control over configurable aspects of NMEA parsing.
    /// </summary>
    public class NmeaParserOptions
    {
        /// <summary>
        /// Gets or sets a value indicating whether the presence of non-standard or unsupported
        /// fields in an NMEA tag block should be treated as a non-recoverable error.
        /// </summary>
        public bool ThrowWhenTagBlockContainsUnknownFields { get; set; } = true;
    }
}
