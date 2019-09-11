// <copyright file="TalkerId.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Ais.Net
{
    /// <summary>
    /// The kind of station that originated the message.
    /// </summary>
    public enum TalkerId
    {
        /// <summary>
        /// <c>AB</c>
        /// </summary>
        BaseStation,

        /// <summary>
        /// <c>AD</c>
        /// </summary>
        DependentBaseStation,

        /// <summary>
        /// <c>AI</c>
        /// </summary>
        MobileStation,

        /// <summary>
        /// <c>AN</c>
        /// </summary>
        AidToNavigationStation,

        /// <summary>
        /// <c>AR</c>
        /// </summary>
        ReceivingStation,

        /// <summary>
        /// <c>AS</c>
        /// </summary>
        LimitedBaseStation,

        /// <summary>
        /// <c>AT</c>
        /// </summary>
        TransmittingStation,

        /// <summary>
        /// <c>AX</c>
        /// </summary>
        RepeaterStation,

        /// <summary>
        /// <c>BS</c>
        /// </summary>
        DeprecatedBaseStation,

        /// <summary>
        /// <c>SA</c>
        /// </summary>
        PhysicalShoreStation,
    }
}