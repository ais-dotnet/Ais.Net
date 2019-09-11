// <copyright file="NavigationStatus.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Ais.Net
{
    /// <summary>
    /// The navigation status of a vessel.
    /// </summary>
    public enum NavigationStatus
    {
        /// <summary>
        /// Under way using engine.
        /// </summary>
        UnderwayUsingEngine = 0,

        /// <summary>
        /// At anchjor.
        /// </summary>
        AtAnchor = 1,

        /// <summary>
        /// Not under command.
        /// </summary>
        NotUnderCommand = 2,

        /// <summary>
        /// Restricted manoeverability.
        /// </summary>
        RestrictedManoeuverability = 3,

        /// <summary>
        /// Constrained by her draught.
        /// </summary>
        ConstrainedByHerDraught = 4,

        /// <summary>
        /// Moored.
        /// </summary>
        Moored = 5,

        /// <summary>
        /// Aground.
        /// </summary>
        Aground = 6,

        /// <summary>
        /// Engaged in fishing.
        /// </summary>
        EngagedInFishing = 7,

        /// <summary>
        /// Under way sailing.
        /// </summary>
        UnderWaySailing = 8,

        /// <summary>
        /// Reserved for future amendment of Navigational Status for HSC.
        /// </summary>
        ReservedForFutureAmendmentOfNavigationalStatusForHsc = 9,

        /// <summary>
        /// Reserved for future amendment of Navigational Status for WIG.
        /// </summary>
        ReservedForFutureAmendmentOfNavigationalStatusForWig = 10,

        /// <summary>
        /// Not currently used.
        /// </summary>
        ReservedForFutureUse11 = 11,

        /// <summary>
        /// Not currently used.
        /// </summary>
        ReservedForFutureUse12 = 12,

        /// <summary>
        /// Not currently used.
        /// </summary>
        ReservedForFutureUse13 = 13,

        /// <summary>
        /// AIS-SART is active.
        /// </summary>
        AisSartIsActive = 14,

        /// <summary>
        /// Information not available.
        /// </summary>
        NotDefined = 15,
    }
}