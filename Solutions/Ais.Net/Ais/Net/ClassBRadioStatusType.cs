// <copyright file="ClassBRadioStatusType.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Ais.Net
{
    /// <summary>
    /// Indicates which type of radio status block is present in a Class B message.
    /// </summary>
    public enum ClassBRadioStatusType
    {
        /// <summary>
        /// Self-Organized Time-Division Multiple Access.
        /// </summary>
        Sotdma = 0,

        /// <summary>
        /// Incremental Time-Division Multiple Access.
        /// </summary>
        Itdma = 1,
    }
}