// <copyright file="RadioSyncState.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Ais.Net
{
    /// <summary>
    /// Describes the time synchronization mechanism in use by the message originator.
    /// </summary>
    public enum RadioSyncState
    {
        /// <summary>
        /// The station has direct access to a UTC timing source with sufficient accuracy.
        /// </summary>
        UtcDirect = 0,

        /// <summary>
        /// The station does not have direct access to a UTC timing source, and is instead
        /// receiving UTC timing from some other station that does have direct access.
        /// </summary>
        UtcIndirect = 1,

        /// <summary>
        /// The station does not have direct access to a UTC timing source, but is in contact
        /// with at least one base station, and is instead using timing information reported by
        /// the station it can see that is in communication with the highest number of other
        /// stations.
        /// </summary>
        ToBaseStation = 2,

        /// <summary>
        /// The station does not have direct access to a UTC timing source, is not receiving
        /// timing information from another station that does have direct access, and is not
        /// in communication with a base station from which it can receive timing information,
        /// and is instead has fallen back to receiving timing information the station reporting
        /// the highest number of other receivers that it is in communication with.
        /// </summary>
        ToOtherStation = 3,
    }
}