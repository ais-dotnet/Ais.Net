// <copyright file="AisStrings.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Ais.Net
{
    /// <summary>
    /// Functions for working with strings in AIS messages.
    /// </summary>
    public static class AisStrings
    {
        /// <summary>
        /// Converts a 6-bit value from an AIS message representing a character to its
        /// corresponding ASCII value.
        /// </summary>
        /// <param name="aisCharacter">
        /// The character as encoded in a 6-bit field in an AIS message.
        /// </param>
        /// <returns>The ASCII value.</returns>
        public static byte AisCharacterToAsciiValue(byte aisCharacter) =>
            aisCharacter < 32
            ? (byte)(aisCharacter + '@')
            : aisCharacter;
    }
}