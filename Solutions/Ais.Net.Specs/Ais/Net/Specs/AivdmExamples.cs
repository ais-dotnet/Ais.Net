// <copyright file="AivdmExamples.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>
//
// Contains data under the Norwegian licence for Open Government data (NLOD) distributed by
// the Norwegian Costal Administration - https://ais.kystverket.no/
// The license can be found at https://data.norge.no/nlod/en/2.0
// The lines in this file that contain data from this source are annotated with a comment containing "ais.kystverket.no"
// The NLOD applies only to the data in these annotated lines. The license under which you are using this software
// (either the AGPLv3, or a commercial license) applies to the whole file.

namespace Ais.Net.Specs
{
    internal static class AivdmExamples
    {
        public const string SomeSortOfAivdmMessageRenameThisWhenWeKnowMore = "!AIVDM,1,1,,A,B3m:H900AP@b:79ae6:<OwnUoP06,0*78"; // ais.kystverket.no
        public const string SimpleTagBlockWithoutDelimiters = "s:42,c:1567684904*38";
        public const string SomeSortOfAivdmMessageRenameThisWhenWeKnowMoreWithTagBlock = @"\" + SimpleTagBlockWithoutDelimiters + @"\" + SomeSortOfAivdmMessageRenameThisWhenWeKnowMore;
        public const string MessageWithTaAGPLaceholderFormat = @"\s:42,c:1567684904*38\!{0},1,1,,A,B3m:H900AP@b:79ae6:<OwnUoP06,2*0A";   // ais.kystverket.no
        public const string MessageFragmentFormat = @"\g:{0}-{1}-{3},s:42,c:1567684904*4E\!AIVDM,{1},{0},{2},A,53mSc8400000h<pH0008E8qBm=@DTp580000000N1P614t0Ht7P000000000,0*01";  // ais.kystverket.no
        public const string MessageFragmentWithoutGroupInHeaderFormat = @"\s:42,c:1567684904*4E\!AIVDM,{1},{0},{2},A,53mSc8400000h<pH0008E8qBm=@DTp580000000N1P614t0Ht7P000000000,0*01";  // ais.kystverket.no
        public const string NonFragmentedMessage = @"\s:42,c:1567684904*38\!AIVDM,1,1,,A,B3m:H900AP@b:79ae6:<OwnUoP06,0*78";    // ais.kystverket.no
        public const string MessageWithRadioChannelPlaceholderFormat = "!AIVDM,1,1,,{0},B3m:H900AP@b:79ae6:<OwnUoP06,2*0A";     // ais.kystverket.no
        public const string MessageWithPayloadPlaceholderFormat = "!AIVDM,1,1,,A,{0},2*0A";
        public const string MessageWithPaddinAGPLaceholderFormat = "!AIVDM,1,1,,A,B3m:H900AP@b:79ae6:<OwnUoP06,{0}*0A";          // ais.kystverket.no
    }
}