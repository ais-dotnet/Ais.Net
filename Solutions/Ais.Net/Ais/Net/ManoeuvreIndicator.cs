// <copyright file="ManoeuvreIndicator.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Ais.Net
{
    /// <summary>
    /// Indicates whether a vessel is engaged in a special manoeuvre.
    /// </summary>
    public enum ManoeuvreIndicator
    {
        /// <summary>
        /// Information not available.
        /// </summary>
        /// <remarks>
        /// This is marked as 'default' in http://catb.org/gpsd/AIVDM.html although it's not clear
        /// what that means, since the field is mandatory.
        /// </remarks>
        NotAvailable = 0,

        /// <summary>
        /// Vessel not engaged in a special manoeuvre.
        /// </summary>
        NoSpecialManoeuvre = 1,

        /// <summary>
        /// Vessel engaged in a special manoeuvre (e.g., a regional passing arrangement).
        /// </summary>
        SpecialManoeuvre = 2,

        /// <summary>
        /// A value that doesn't seem to have a defined purpose, but crops up in real anyway.
        /// </summary>
        NotDefinedBySpec = 3,
    }
}