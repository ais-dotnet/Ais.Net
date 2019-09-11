// <copyright file="PositionReportClassBParserSpecsSteps.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Ais.Net.Specs.AisMessageTypes
{
    using System.Text;
    using Ais.Net;
    using NUnit.Framework;
    using TechTalk.SpecFlow;

    [Binding]
    public class PositionReportClassBParserSpecsSteps
    {
        private ParserMaker makeParser;

        private delegate NmeaAisPositionReportClassBParser ParserMaker();

        private delegate void ParserTest(NmeaAisPositionReportClassBParser parser);

        [When("I parse '(.*)' with padding (.*) as a Position Report Class B")]
        public void WhenIParseWithPaddingAsAPositionReportClassB(string payload, uint padding)
        {
            this.When(() => new NmeaAisPositionReportClassBParser(Encoding.ASCII.GetBytes(payload), padding));
        }

        [Then(@"AisPositionReportClassBParser\.Type is (.*)")]
        public void ThenAisPositionReportClassBParser_TypeIs(int messageType)
        {
            this.Then(parser => Assert.AreEqual(messageType, parser.MessageType));
        }

        [Then(@"AisPositionReportClassBParser\.RepeatIndicator is (.*)")]
        public void ThenAisPositionReportClassBParser_RepeatIndicatorIs(int repeatCount)
        {
            this.Then(parser => Assert.AreEqual(repeatCount, parser.RepeatIndicator));
        }

        [Then(@"AisPositionReportClassBParser\.Mmsi is (.*)")]
        public void ThenAisPositionReportClassBParser_MmsiIs(int mmsi)
        {
            this.Then(parser => Assert.AreEqual(mmsi, parser.Mmsi));
        }

        [Then(@"AisPositionReportClassBParser\.RegionalReserved38 is (.*)")]
        public void ThenAisPositionReportClassBParser_RegionalReservedIs(int reserved)
        {
            this.Then(parser => Assert.AreEqual(reserved, parser.RegionalReserved38));
        }

        [Then(@"AisPositionReportClassBParser\.SpeedOverGroundTenths is (.*)")]
        public void ThenAisPositionReportClassBParser_SpeedOverGroundTenthsIs(int speedOverGround)
        {
            this.Then(parser => Assert.AreEqual(speedOverGround, parser.SpeedOverGroundTenths));
        }

        [Then(@"AisPositionReportClassBParser\.PositionAccuracy is (.*)")]
        public void ThenAisPositionReportClassBParser_PositionAccuracyIs(bool positionAccuracy)
        {
            this.Then(parser => Assert.AreEqual(positionAccuracy, parser.PositionAccuracy));
        }

        [Then(@"AisPositionReportClassBParser\.Longitude10000thMins is (.*)")]
        public void ThenAisPositionReportClassBParser_LongitudeIs(int longitude)
        {
            this.Then(parser => Assert.AreEqual(longitude, parser.Longitude10000thMins));
        }

        [Then(@"AisPositionReportClassBParser\.Latitude10000thMins is (.*)")]
        public void ThenAisPositionReportClassBParser_LatitudeIs(int latitude)
        {
            this.Then(parser => Assert.AreEqual(latitude, parser.Latitude10000thMins));
        }

        [Then(@"AisPositionReportClassBParser\.CourseOverGround10thDegrees is (.*)")]
        public void ThenAisPositionReportClassBParser_CourseOverGroundIs(int courseOverGround)
        {
            this.Then(parser => Assert.AreEqual(courseOverGround, parser.CourseOverGround10thDegrees));
        }

        [Then(@"AisPositionReportClassBParser\.TrueHeadingDegrees is (.*)")]
        public void ThenAisPositionReportClassBParser_TrueHeadingDegreesIs(int trueHeading)
        {
            this.Then(parser => Assert.AreEqual(trueHeading, parser.TrueHeadingDegrees));
        }

        [Then(@"AisPositionReportClassBParser\.TimeStampSecond is (.*)")]
        public void ThenAisPositionReportClassBParser_TimeStampSecondIs(int timeStamp)
        {
            this.Then(parser => Assert.AreEqual(timeStamp, parser.TimeStampSecond));
        }

        [Then(@"AisPositionReportClassBParser\.RegionalReserved139 is (.*)")]
        public void ThenAisPositionReportClassBParser_RegionalReserved139Is(int reserved)
        {
            this.Then(parser => Assert.AreEqual(reserved, parser.RegionalReserved139));
        }

        [Then(@"AisPositionReportClassBParser\.CsUnit is (.*)")]
        public void ThenAisPositionReportClassBParser_CsUnitIsSotdma(ClassBUnit unit)
        {
            this.Then(parser => Assert.AreEqual(unit, parser.CsUnit));
        }

        [Then(@"AisPositionReportClassBParser\.HasDisplay is (.*)")]
        public void ThenAisPositionReportClassBParser_HasDisplayIs(bool hasDisplay)
        {
            this.Then(parser => Assert.AreEqual(hasDisplay, parser.HasDisplay));
        }

        [Then(@"AisPositionReportClassBParser\.IsDscAttached is (.*)")]
        public void ThenAisPositionReportClassBParser_IsDscAttached(bool isDscAttached)
        {
            this.Then(parser => Assert.AreEqual(isDscAttached, parser.IsDscAttached));
        }

        [Then(@"AisPositionReportClassBParser\.CanSwitchBands is (.*)")]
        public void ThenAisPositionReportClassBParser_CanSwitchBands(bool canSwitchBands)
        {
            this.Then(parser => Assert.AreEqual(canSwitchBands, parser.CanSwitchBands));
        }

        [Then(@"AisPositionReportClassBParser\.CanAcceptMessage22ChannelAssignment is (.*)")]
        public void ThenAisPositionReportClassBParser_CanAcceptMessage22ChannelAssignment(bool canAcceptMessage22ChannelAssignment)
        {
            this.Then(parser => Assert.AreEqual(canAcceptMessage22ChannelAssignment, parser.CanAcceptMessage22ChannelAssignment));
        }

        [Then(@"AisPositionReportClassBParser\.IsAssigned is (.*)")]
        public void ThenAisPositionReportClassBParser_IsAssigned(bool isAssigned)
        {
            this.Then(parser => Assert.AreEqual(isAssigned, parser.IsAssigned));
        }

        [Then(@"AisPositionReportClassBParser\.RaimFlag is (.*)")]
        public void ThenAisPositionReportClassBParser_RaimFlag(bool raim)
        {
            this.Then(parser => Assert.AreEqual(raim, parser.RaimFlag));
        }

        [Then(@"AisPositionReportClassBParser\.RadioStatusType is (.*)")]
        public void ThenAisPositionReportClassBParser_RadioStatusTypeIsSOTDMA(ClassBRadioStatusType radioStatusType)
        {
            this.Then(parser => Assert.AreEqual(radioStatusType, parser.RadioStatusType));
        }

        private void When(ParserMaker makeParser)
        {
            this.makeParser = makeParser;
        }

        private void Then(ParserTest test)
        {
            NmeaAisPositionReportClassBParser parser = this.makeParser();
            test(parser);
        }
    }
}