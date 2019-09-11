// <copyright file="VesselDataOrigin.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Ais.Net
{
    /// <summary>
    /// The origin of the data.
    /// </summary>
    public enum VesselDataOrigin
    {
        /// <summary>
        /// Information received from other vessels or stations.
        /// </summary>
        Vdm,

        /// <summary>
        /// Information on your own vessel.
        /// </summary>
        Vdo,
    }
}