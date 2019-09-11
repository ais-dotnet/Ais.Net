// <copyright file="PositionReportClassAParserSpecsSteps.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Ais.Net.Specs.AisMessageTypes
{
    using System.Text;
    using NUnit.Framework;
    using TechTalk.SpecFlow;

    [Binding]
    public class PositionReportClassAParserSpecsSteps
    {
        private ParserMaker makeParser;

        private delegate NmeaAisPositionReportClassAParser ParserMaker();

        private delegate void ParserTest(NmeaAisPositionReportClassAParser parser);

        [When("I parse '(.*)' with padding (.*) as a Position Report Class A")]
        public void WhenIParseWithPaddingAsAPositionReportClassA(string payload, uint padding)
        {
            this.When(() => new NmeaAisPositionReportClassAParser(Encoding.ASCII.GetBytes(payload), padding));
        }

        [Then(@"AisPositionReportClassAParser\.Type is (.*)")]
        public void ThenAisPositionReportClassAParser_TypeIs(int messageType)
        {
            this.Then(parser => Assert.AreEqual(messageType, parser.MessageType));
        }

        [Then(@"AisPositionReportClassAParser\.RepeatIndicator is (.*)")]
        public void ThenAisPositionReportClassAParser_RepeatIndicatorIs(int repeatCount)
        {
            this.Then(parser => Assert.AreEqual(repeatCount, parser.RepeatIndicator));
        }

        [Then(@"AisPositionReportClassAParser\.Mmsi is '(.*)'")]
        public void ThenAisPositionReportClassAParser_MmsiIs(int mmsi)
        {
            this.Then(parser => Assert.AreEqual(mmsi, parser.Mmsi));
        }

        [Then(@"AisPositionReportClassAParser\.NavigationStatus is (.*)")]
        public void ThenAisPositionReportClassAParser_NavigationStatusIs(NavigationStatus navigationStatus)
        {
            this.Then(parser => Assert.AreEqual(navigationStatus, parser.NavigationStatus));
        }

        [Then(@"AisPositionReportClassAParser\.RateOfTurn is (.*)")]
        public void ThenAisPositionReportClassAParser_RateOfTurnIs(int rateOfTurn)
        {
            this.Then(parser => Assert.AreEqual(rateOfTurn, parser.RateOfTurn));
        }

        [Then(@"AisPositionReportClassAParser\.SpeedOverGroundTenths is (.*)")]
        public void ThenAisPositionReportClassAParser_SpeedOverGroundTenthsIs(int speedOverGround)
        {
            this.Then(parser => Assert.AreEqual(speedOverGround, parser.SpeedOverGroundTenths));
        }

        [Then(@"AisPositionReportClassAParser\.PositionAccuracy is (.*)")]
        public void ThenAisPositionReportClassAParser_PositionAccuracyIsTrue(bool positionAccuracy)
        {
            this.Then(parser => Assert.AreEqual(positionAccuracy, parser.PositionAccuracy));
        }

        [Then(@"AisPositionReportClassAParser\.Longitude10000thMins is (.*)")]
        public void ThenAisPositionReportClassAParser_LongitudeIs(int longitude)
        {
            this.Then(parser => Assert.AreEqual(longitude, parser.Longitude10000thMins));
        }

        [Then(@"AisPositionReportClassAParser\.Latitude10000thMins is (.*)")]
        public void ThenAisPositionReportClassAParser_LatitudeIs(int latitude)
        {
            this.Then(parser => Assert.AreEqual(latitude, parser.Latitude10000thMins));
        }

        [Then(@"AisPositionReportClassAParser\.CourseOverGround10thDegrees is (.*)")]
        public void ThenAisPositionReportClassAParser_CourseOverGroundthDegreesIs(int courseOverGround)
        {
            this.Then(parser => Assert.AreEqual(courseOverGround, parser.CourseOverGround10thDegrees));
        }

        [Then(@"AisPositionReportClassAParser\.TrueHeadingDegrees is (.*)")]
        public void ThenAisPositionReportClassAParser_TrueHeadingDegreesIs(int trueHeading)
        {
            this.Then(parser => Assert.AreEqual(trueHeading, parser.TrueHeadingDegrees));
        }

        [Then(@"AisPositionReportClassAParser\.TimeStampSecond is (.*)")]
        public void ThenAisPositionReportClassAParser_TimeStampSecondIs(int timeStamp)
        {
            this.Then(parser => Assert.AreEqual(timeStamp, parser.TimeStampSecond));
        }

        [Then(@"AisPositionReportClassAParser\.ManoeuvreIndicator is (.*)")]
        public void ThenAisPositionReportClassAParser_ManoeuvreIndicatorIsNotAvailable(ManoeuvreIndicator manoeuvre)
        {
            this.Then(parser => Assert.AreEqual(manoeuvre, parser.ManoeuvreIndicator));
        }

        [Then(@"AisPositionReportClassAParser\.SpareBits145 is (.*)")]
        public void ThenAisPositionReportClassAParser_SpareBitsIs(int value)
        {
            this.Then(parser => Assert.AreEqual(value, parser.SpareBits145));
        }

        [Then(@"AisPositionReportClassAParser\.RaimFlag is (.*)")]
        public void ThenAisPositionReportClassAParser_RaimFlagIs(bool value)
        {
            this.Then(parser => Assert.AreEqual(value, parser.RaimFlag));
        }

        [Then(@"AisPositionReportClassAParser\.RadioSyncState is (.*)")]
        public void ThenAisPositionReportClassAParser_RadioSyncStateIsUtcDirect(RadioSyncState state)
        {
            this.Then(parser => Assert.AreEqual(state, parser.RadioSyncState));
        }

        [Then(@"AisPositionReportClassAParser\.RadioSlotTimeout is (.*)")]
        public void ThenAisPositionReportClassAParser_RadioSlotTimeoutIs(uint timeout)
        {
            this.Then(parser => Assert.AreEqual(timeout, parser.RadioSlotTimeout));
        }

        [Then(@"AisPositionReportClassAParser\.RadioSubMessage is (.*)")]
        public void ThenAisPositionReportClassAParser_RadioSubMessageIs(uint message)
        {
            this.Then(parser => Assert.AreEqual(message, parser.RadioSubMessage));
        }

        private void When(ParserMaker makeParser)
        {
            this.makeParser = makeParser;
        }

        private void Then(ParserTest test)
        {
            NmeaAisPositionReportClassAParser parser = this.makeParser();
            test(parser);
        }
    }
}