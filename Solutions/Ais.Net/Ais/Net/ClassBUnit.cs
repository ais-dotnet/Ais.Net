// <copyright file="ClassBUnit.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Ais.Net
{
    /// <summary>
    /// Indicates which type of unit a Class B CS Position Report originated from.
    /// </summary>
    /// <remarks>
    /// Class B AIS equipment comes in two forms. The cheapest uses CSTDMA (Carrier Sense
    /// Time-Division Multiple Access), meaning that it waits until nobody else seems to be
    /// broadcasting, and then broadcasts in the hope that the transmission won't collide.
    /// A more expensive kind, sometimes called Class B+, uses the same SOTMA (Self-Organized
    /// Time-Division Multiple Access) mechanism as Class A transmitters.
    /// </remarks>
    public enum ClassBUnit
    {
        /// <summary>
        /// The unit uses self-organised time-division multiple access.
        /// </summary>
        Sotdma = 0,

        /// <summary>
        /// The unit uses carrier sense time-division multiple access.
        /// </summary>
        Cstdma = 1,
    }
}