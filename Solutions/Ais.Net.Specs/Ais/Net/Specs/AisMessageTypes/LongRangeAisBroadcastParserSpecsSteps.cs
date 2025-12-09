// <copyright file="LongRangeAisBroadcastParserSpecsSteps.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Ais.Net.Specs.AisMessageTypes
{
    using System.Text;
    using NUnit.Framework;
    using Reqnroll;

    [Binding]
    public class LongRangeAisBroadcastParserSpecsSteps
    {
        private ParserMaker makeParser;

        private delegate NmeaAisLongRangeAisBroadcastParser ParserMaker();

        private delegate void ParserTest(NmeaAisLongRangeAisBroadcastParser parser);

        [When("I parse '(.*)' with padding (.*) as a Long Range Ais Broadcast")]
        public void WhenIParseWithPaddingAsALongRangeAisBroadcast(string payload, uint padding)
        {
            this.When(() => new NmeaAisLongRangeAisBroadcastParser(Encoding.ASCII.GetBytes(payload), padding));
        }

        [Then(@"NmeaAisLongRangeAisBroadcastParser\.Type is (.*)")]
        public void ThenNmeaAisLongRangeAisBroadcastParser_TypeIs(int messageType)
        {
            this.Then(parser => Assert.AreEqual(messageType, parser.MessageType));
        }

        [Then(@"NmeaAisLongRangeAisBroadcastParser\.RepeatIndicator is (.*)")]
        public void ThenNmeaAisLongRangeAisBroadcastParser_RepeatIndicatorIs(int repeatCount)
        {
            this.Then(parser => Assert.AreEqual(repeatCount, parser.RepeatIndicator));
        }

        [Then(@"NmeaAisLongRangeAisBroadcastParser\.Mmsi is (.*)")]
        public void ThenNmeaAisLongRangeAisBroadcastParser_MmsiIs(int mmsi)
        {
            this.Then(parser => Assert.AreEqual(mmsi, parser.Mmsi));
        }

        [Then(@"NmeaAisLongRangeAisBroadcastParser\.PositionAccuracy is (.*)")]
        public void ThenNmeaAisLongRangeAisBroadcastParser_PositionAccuracyIs(bool positionAccuracy)
        {
            this.Then(parser => Assert.AreEqual(positionAccuracy, parser.PositionAccuracy));
        }

        [Then(@"NmeaAisLongRangeAisBroadcastParser\.RaimFlag is (.*)")]
        public void ThenNmeaAisLongRangeAisBroadcastParser_RaimFlagIs(bool value)
        {
            this.Then(parser => Assert.AreEqual(value, parser.RaimFlag));
        }

        [Then(@"NmeaAisLongRangeAisBroadcastParser\.NavigationStatus is (.*)")]
        public void ThenNmeaAisLongRangeAisBroadcastParser_NavigationStatusIs(NavigationStatus navigationStatus)
        {
            this.Then(parser => Assert.AreEqual(navigationStatus, parser.NavigationStatus));
        }

        [Then(@"NmeaAisLongRangeAisBroadcastParser\.Longitude10thMins is (.*)")]
        public void ThenNmeaAisLongRangeAisBroadcastParser_LongitudeIs(int longitude)
        {
            this.Then(parser => Assert.AreEqual(longitude, parser.Longitude10thMins));
        }

        [Then(@"NmeaAisLongRangeAisBroadcastParser\.Latitude10thMins is (.*)")]
        public void ThenNmeaAisLongRangeAisBroadcastParser_LatitudeIs(int latitude)
        {
            this.Then(parser => Assert.AreEqual(latitude, parser.Latitude10thMins));
        }

        [Then(@"NmeaAisLongRangeAisBroadcastParser\.SpeedOverGroundTenths is (.*)")]
        public void ThenNmeaAisLongRangeAisBroadcastParser_SpeedOverGroundTenthsIs(int speedOverGround)
        {
            this.Then(parser => Assert.AreEqual(speedOverGround, parser.SpeedOverGroundTenths));
        }

        [Then(@"NmeaAisLongRangeAisBroadcastParser\.CourseOverGroundDegrees is (.*)")]
        public void ThenNmeaAisLongRangeAisBroadcastParser_CourseOverGroundDegreesIs(int courseOverGround)
        {
            this.Then(parser => Assert.AreEqual(courseOverGround, parser.CourseOverGroundDegrees));
        }

        [Then(@"NmeaAisLongRangeAisBroadcastParser\.NotGnssPosition is (.*)")]
        public void ThenNmeaAisLongRangeAisBroadcastParser_NotGnssPositionIs(bool gnssStatus)
        {
            this.Then(parser => Assert.AreEqual(gnssStatus, parser.NotGnssPosition));
        }

        [Then(@"NmeaAisLongRangeAisBroadcastParser\.Spare95 is (.*)")]
        public void ThenNmeaAisLongRangeAisBroadcastParser_SpareIsFalse(bool flag)
        {
            this.Then(parser => Assert.AreEqual(flag, parser.Spare95));
        }

        private void When(ParserMaker makeParser)
        {
            this.makeParser = makeParser;
        }

        private void Then(ParserTest test)
        {
            NmeaAisLongRangeAisBroadcastParser parser = this.makeParser();
            test(parser);
        }
    }
}
