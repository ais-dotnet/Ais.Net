// <copyright file="StaticAndVoyageRelatedDataParserSpecsSteps.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Ais.Net.Specs.AisMessageTypes
{
    using System.Text;
    using NUnit.Framework;
    using TechTalk.SpecFlow;

    [Binding]
    public class StaticAndVoyageRelatedDataParserSpecsSteps
    {
        private ParserMaker makeParser;

        private delegate NmeaAisStaticAndVoyageRelatedDataParser ParserMaker();

        private delegate void ParserTest(NmeaAisStaticAndVoyageRelatedDataParser parser);

        [When("I parse '(.*)' with padding (.*) as Static and Voyage Related Data")]
        public void WhenIParseWithPaddingAsStaticAndVoyageRelatedData(string payload, uint padding)
        {
            this.When(() => new NmeaAisStaticAndVoyageRelatedDataParser(Encoding.ASCII.GetBytes(payload), padding));
        }

        [Then(@"NmeaAisStaticAndVoyageRelatedDataParser\.Type is (.*)")]
        public void ThenNmeaAisStaticAndVoyageRelatedDataParser_TypeIs(uint messageType)
        {
            this.Then(parser => Assert.AreEqual(messageType, parser.MessageType));
        }

        [Then(@"NmeaAisStaticAndVoyageRelatedDataParser\.RepeatIndicator is (.*)")]
        public void ThenNmeaAisStaticAndVoyageRelatedDataParser_RepeatIndicatorIs(uint repeatCount)
        {
            this.Then(parser => Assert.AreEqual(repeatCount, parser.RepeatIndicator));
        }

        [Then(@"NmeaAisStaticAndVoyageRelatedDataParser\.Mmsi is (.*)")]
        public void ThenNmeaAisStaticAndVoyageRelatedDataParser_MmsiIs(int mmsi)
        {
            this.Then(parser => Assert.AreEqual(mmsi, parser.Mmsi));
        }

        [Then(@"NmeaAisStaticAndVoyageRelatedDataParser\.AisVersion is (.*)")]
        public void ThenNmeaAisStaticAndVoyageRelatedDataParser_AisVersionIs(int aisVersion)
        {
            this.Then(parser => Assert.AreEqual(aisVersion, parser.AisVersion));
        }

        [Then(@"NmeaAisStaticAndVoyageRelatedDataParser\.ImoNumber is (.*)")]
        public void ThenNmeaAisStaticAndVoyageRelatedDataParser_ImoNumberIs(int imoNumber)
        {
            this.Then(parser => Assert.AreEqual(imoNumber, parser.ImoNumber));
        }

        [Then(@"NmeaAisStaticAndVoyageRelatedDataParser\.CallSign is (.*)")]
        public void ThenNmeaAisStaticAndVoyageRelatedDataParser_CallSignIs(string callSign)
        {
            this.Then(parser => AisStringsSpecsSteps.TestString(callSign, 7, parser.CallSign));
        }

        [Then(@"NmeaAisStaticAndVoyageRelatedDataParser\.VesselName is (.*)")]
        public void ThenNmeaAisStaticAndVoyageRelatedDataParser_VesselNameIs(string vesselName)
        {
            this.Then(parser => AisStringsSpecsSteps.TestString(vesselName, 20, parser.VesselName));
        }

        [Then(@"NmeaAisStaticAndVoyageRelatedDataParser\.ShipType is (.*)")]
        public void ThenNmeaAisStaticAndVoyageRelatedDataParser_ShipTypeIs(ShipType type)
        {
            this.Then(parser => Assert.AreEqual(type, parser.ShipType));
        }

        [Then(@"NmeaAisStaticAndVoyageRelatedDataParser\.DimensionToBow is (.*)")]
        public void ThenNmeaAisStaticAndVoyageRelatedDataParser_DimensionToBowIs(int size)
        {
            this.Then(parser => Assert.AreEqual(size, parser.DimensionToBow));
        }

        [Then(@"NmeaAisStaticAndVoyageRelatedDataParser\.DimensionToStern is (.*)")]
        public void ThenNmeaAisStaticAndVoyageRelatedDataParser_DimensionToSternIs(int size)
        {
            this.Then(parser => Assert.AreEqual(size, parser.DimensionToStern));
        }

        [Then(@"NmeaAisStaticAndVoyageRelatedDataParser\.DimensionToPort is (.*)")]
        public void ThenNmeaAisStaticAndVoyageRelatedDataParser_DimensionToPortIs(int size)
        {
            this.Then(parser => Assert.AreEqual(size, parser.DimensionToPort));
        }

        [Then(@"NmeaAisStaticAndVoyageRelatedDataParser\.DimensionToStarboard is (.*)")]
        public void ThenNmeaAisStaticAndVoyageRelatedDataParser_DimensionToStarboardIs(int size)
        {
            this.Then(parser => Assert.AreEqual(size, parser.DimensionToStarboard));
        }

        [Then(@"NmeaAisStaticAndVoyageRelatedDataParser\.PositionFixType is (.*)")]
        public void ThenNmeaAisStaticAndVoyageRelatedDataParser_PositionFixTypeIsUndefined(EpfdFixType epfd)
        {
            this.Then(parser => Assert.AreEqual(epfd, parser.PositionFixType));
        }

        [Then(@"NmeaAisStaticAndVoyageRelatedDataParser\.EtaMonth is (.*)")]
        public void ThenNmeaAisStaticAndVoyageRelatedDataParser_EtaMonthIs(int month)
        {
            this.Then(parser => Assert.AreEqual(month, parser.EtaMonth));
        }

        [Then(@"NmeaAisStaticAndVoyageRelatedDataParser\.EtaDay is (.*)")]
        public void ThenNmeaAisStaticAndVoyageRelatedDataParser_EtaDayIs(int day)
        {
            this.Then(parser => Assert.AreEqual(day, parser.EtaDay));
        }

        [Then(@"NmeaAisStaticAndVoyageRelatedDataParser\.EtaHour is (.*)")]
        public void ThenNmeaAisStaticAndVoyageRelatedDataParser_EtaHourIs(int hour)
        {
            this.Then(parser => Assert.AreEqual(hour, parser.EtaHour));
        }

        [Then(@"NmeaAisStaticAndVoyageRelatedDataParser\.EtaMinute is (.*)")]
        public void ThenNmeaAisStaticAndVoyageRelatedDataParser_EtaMinuteIs(int minute)
        {
            this.Then(parser => Assert.AreEqual(minute, parser.EtaMinute));
        }

        [Then(@"NmeaAisStaticAndVoyageRelatedDataParser\.Draught10thMetres is (.*)")]
        public void ThenNmeaAisStaticAndVoyageRelatedDataParser_DraughtthMetresIs(int draught)
        {
            this.Then(parser => Assert.AreEqual(draught, parser.Draught10thMetres));
        }

        [Then(@"NmeaAisStaticAndVoyageRelatedDataParser\.Destination is (.*)")]
        public void ThenNmeaAisStaticAndVoyageRelatedDataParser_DestinationIs(string destination)
        {
            this.Then(parser => AisStringsSpecsSteps.TestString(destination, 20, parser.Destination));
        }

        [Then(@"NmeaAisStaticAndVoyageRelatedDataParser\.DteNotReady is (.*)")]
        public void ThenNmeaAisStaticAndVoyageRelatedDataParser_DteNotReadyIsFalse(bool notReady)
        {
            this.Then(parser => Assert.AreEqual(notReady, parser.IsDteNotReady));
        }

        [Then(@"NmeaAisStaticAndVoyageRelatedDataParser\.Spare423 is (.*)")]
        public void ThenNmeaAisStaticAndVoyageRelatedDataParser_SpareIs(int spare)
        {
            this.Then(parser => Assert.AreEqual(spare, parser.Spare423));
        }

        private void When(ParserMaker makeParser)
        {
            this.makeParser = makeParser;
        }

        private void Then(ParserTest test)
        {
            NmeaAisStaticAndVoyageRelatedDataParser parser = this.makeParser();
            test(parser);
        }
    }
}