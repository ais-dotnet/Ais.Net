// <copyright file="StaticDataReportParserSpecsSteps.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Ais.Net.Specs.AisMessageTypes
{
    using System;
    using System.Text;
    using NUnit.Framework;
    using Reqnroll;

    [Binding]
    public class StaticDataReportParserSpecsSteps
    {
        private InitialParserMaker makeInitialParser;
        private PartAParserMaker makePartAParser;
        private PartBParserMaker makePartBParser;
        private Exception exception;

        private delegate uint InitialParserMaker();

        private delegate NmeaAisStaticDataReportParserPartA PartAParserMaker();

        private delegate NmeaAisStaticDataReportParserPartB PartBParserMaker();

        private delegate void InitialParserTest(uint partNumber);

        private delegate void PartAParserTest(NmeaAisStaticDataReportParserPartA parser);

        private delegate void PartBParserTest(NmeaAisStaticDataReportParserPartB parser);

        [When("I inspect the Static Data Report part of '(.*)' with padding (.*)")]
        public void WhenIInspectTheStaticDataReportPartOfWithPadding(string payload, uint padding)
        {
            this.WhenInitial(() => NmeaAisStaticDataReportParser.GetPartNumber(Encoding.ASCII.GetBytes(payload), padding));
        }

        [When("I parse '(.*)' with padding (.*) as Static Data Report Part A")]
        public void WhenIParseWithPaddingAsStaticDataReportPartA(string payload, uint padding)
        {
            this.WhenPartA(() => new NmeaAisStaticDataReportParserPartA(Encoding.ASCII.GetBytes(payload), padding));
        }

        [When("I parse '(.*)' with padding (.*) as Static Data Report Part B")]
        public void WhenIParseWithPaddingAsStaticDataReportPartB(string payload, uint padding)
        {
            this.WhenPartB(() => new NmeaAisStaticDataReportParserPartB(Encoding.ASCII.GetBytes(payload), padding));
        }

        [When("I parse '(.*)' with padding (.*) as Static Data Report Part A catching exception")]
        public void WhenIParseWithPaddingAsStaticDataReportPartACatchingException(string payload, uint padding)
        {
            try
            {
                new NmeaAisStaticDataReportParserPartA(Encoding.ASCII.GetBytes(payload), padding);
                Assert.Fail("Was expecting an exception");
            }
            catch (Exception x)
            {
                this.exception = x;
            }
        }

        [When("I parse '(.*)' with padding (.*) as Static Data Report Part B catching exception")]
        public void WhenIParseWithPaddingAsStaticDataReportPartBCatchingException(string payload, uint padding)
        {
            try
            {
                new NmeaAisStaticDataReportParserPartB(Encoding.ASCII.GetBytes(payload), padding);
                Assert.Fail("Was expecting an exception");
            }
            catch (Exception x)
            {
                this.exception = x;
            }
        }

        [Then(@"NmeaAisStaticDataReportParser\.GetPartNumber returns (.*)")]
        public void ThenNmeaAisStaticDataReportParser_GetPartNumberReturns(uint partNumber)
        {
            this.ThenInitial(r => Assert.AreEqual(partNumber, r));
        }

        [Then("the constructor throws ArgumentException")]
        public void ThenTheConstructorThrowsArgumentException()
        {
            Assert.IsInstanceOf<ArgumentException>(this.exception);
        }

        [Then(@"NmeaAisStaticDataReportParserPartA\.Type is (.*)")]
        public void ThenNmeaAisStaticDataReportParserPartA_TypeIs(int messageType)
        {
            this.ThenPartA(parser => Assert.AreEqual(messageType, parser.MessageType));
        }

        [Then(@"NmeaAisStaticDataReportParserPartB\.Type is (.*)")]
        public void ThenNmeaAisStaticDataReportParserPartB_TypeIs(int messageType)
        {
            this.ThenPartB(parser => Assert.AreEqual(messageType, parser.MessageType));
        }

        [Then(@"NmeaAisStaticDataReportParserPartA\.RepeatIndicator is (.*)")]
        public void ThenNmeaAisStaticDataReportParserPartA_RepeatIndicatorIs(uint repeatCount)
        {
            this.ThenPartA(parser => Assert.AreEqual(repeatCount, parser.RepeatIndicator));
        }

        [Then(@"NmeaAisStaticDataReportParserPartB\.RepeatIndicator is (.*)")]
        public void ThenNmeaAisStaticDataReportParserPartB_RepeatIndicatorIs(uint repeatCount)
        {
            this.ThenPartB(parser => Assert.AreEqual(repeatCount, parser.RepeatIndicator));
        }

        [Then(@"NmeaAisStaticDataReportParserPartA\.Mmsi is (.*)")]
        public void ThenNmeaAisStaticDataReportParserPartA_MmsiIs(int mmsi)
        {
            this.ThenPartA(parser => Assert.AreEqual(mmsi, parser.Mmsi));
        }

        [Then(@"NmeaAisStaticDataReportParserPartB\.Mmsi is (.*)")]
        public void ThenNmeaAisStaticDataReportParserPartB_MmsiIs(int mmsi)
        {
            this.ThenPartB(parser => Assert.AreEqual(mmsi, parser.Mmsi));
        }

        [Then(@"NmeaAisStaticDataReportParserPartA\.PartNumber is (.*)")]
        public void ThenNmeaAisStaticDataReportParserPartA_PartNumberIs(int partNumber)
        {
            this.ThenPartA(parser => Assert.AreEqual(partNumber, parser.PartNumber));
        }

        [Then(@"NmeaAisStaticDataReportParserPartB\.PartNumber is (.*)")]
        public void ThenNmeaAisStaticDataReportParserPartB_PartNumberIs(int partNumber)
        {
            this.ThenPartB(parser => Assert.AreEqual(partNumber, parser.PartNumber));
        }

        [Then(@"NmeaAisStaticDataReportParserPartA\.VesselName is (.*)")]
        public void ThenNmeaAisStaticDataReportParserPartA_VesselNameIs(string vesselName)
        {
            this.ThenPartA(parser => AisStringsSpecsSteps.TestString(vesselName, 20, parser.VesselName));
        }

        [Then(@"NmeaAisStaticDataReportParserPartA\.Spare is (.*)")]
        public void ThenNmeaAisStaticDataReportParserPartA_SpareIs(uint spare)
        {
            this.ThenPartA(parser => Assert.AreEqual(spare, parser.Spare160));
        }

        [Then(@"NmeaAisStaticDataReportParserPartB\.ShipType is (.*)")]
        public void ThenNmeaAisStaticDataReportParserPartB_ShipTypeIs(ShipType type)
        {
            this.ThenPartB(parser => Assert.AreEqual(type, parser.ShipType));
        }

        [Then(@"NmeaAisStaticDataReportParserPartB\.VendorIdRev3 is (.*)")]
        public void ThenNmeaAisStaticDataReportParserPartB_VendorIdRev3Is(string vendorId)
        {
            this.ThenPartB(parser => AisStringsSpecsSteps.TestString(vendorId, 7, parser.VendorIdRev3));
        }

        [Then(@"NmeaAisStaticDataReportParserPartB\.VendorIdRev4 is (.*)")]
        public void ThenNmeaAisStaticDataReportParserPartB_VendorIdRev4Is(string vendorId)
        {
            this.ThenPartB(parser => AisStringsSpecsSteps.TestString(vendorId, 3, parser.VendorIdRev4));
        }

        [Then(@"NmeaAisStaticDataReportParserPartB\.UnitModelCode is (.*)")]
        public void ThenNmeaAisStaticDataReportParserPartB_UnitModelCodeIs(uint unitModelCode)
        {
            this.ThenPartB(parser => Assert.AreEqual(unitModelCode, parser.UnitModelCode));
        }

        [Then(@"NmeaAisStaticDataReportParserPartB\.SerialNumber is (.*)")]
        public void ThenNmeaAisStaticDataReportParserPartB_SerialNumberIs(uint serialNumber)
        {
            this.ThenPartB(parser => Assert.AreEqual(serialNumber, parser.SerialNumber));
        }

        [Then(@"NmeaAisStaticDataReportParserPartB\.CallSign is (.*)")]
        public void ThenNmeaAisStaticDataReportParserPartB_CallSignIs(string callSign)
        {
            this.ThenPartB(parser => AisStringsSpecsSteps.TestString(callSign, 7, parser.CallSign));
        }

        [Then(@"NmeaAisStaticDataReportParserPartB\.DimensionToBow is (.*)")]
        public void ThenNmeaAisStaticDataReportParserPartB_DimensionToBowIs(uint size)
        {
            this.ThenPartB(parser => Assert.AreEqual(size, parser.DimensionToBow));
        }

        [Then(@"NmeaAisStaticDataReportParserPartB\.DimensionToStern is (.*)")]
        public void ThenNmeaAisStaticDataReportParserPartB_DimensionToSternIs(uint size)
        {
            this.ThenPartB(parser => Assert.AreEqual(size, parser.DimensionToStern));
        }

        [Then(@"NmeaAisStaticDataReportParserPartB\.DimensionToPort is (.*)")]
        public void ThenNmeaAisStaticDataReportParserPartB_DimensionToPortIs(uint size)
        {
            this.ThenPartB(parser => Assert.AreEqual(size, parser.DimensionToPort));
        }

        [Then(@"NmeaAisStaticDataReportParserPartB\.DimensionToStarboard is (.*)")]
        public void ThenNmeaAisStaticDataReportParserPartB_DimensionToStarboardIs(uint size)
        {
            this.ThenPartB(parser => Assert.AreEqual(size, parser.DimensionToStarboard));
        }

        [Then(@"NmeaAisStaticDataReportParserPartB\.MothershipMmsi is (.*)")]
        public void ThenNmeaAisStaticDataReportParserPartB_MothershipMmsiIs(uint mmsi)
        {
            this.ThenPartB(parser => Assert.AreEqual(mmsi, parser.MothershipMmsi));
        }

        [Then(@"NmeaAisStaticDataReportParserPartB\.Spare is (.*)")]
        public void ThenNmeaAisStaticDataReportParserPartB_SpareIs(uint spare)
        {
            this.ThenPartB(parser => Assert.AreEqual(spare, parser.Spare162));
        }

        private void WhenInitial(InitialParserMaker makeParser)
        {
            this.makeInitialParser = makeParser;
        }

        private void WhenPartA(PartAParserMaker makeParser)
        {
            this.makePartAParser = makeParser;
        }

        private void WhenPartB(PartBParserMaker makeParser)
        {
            this.makePartBParser = makeParser;
        }

        private void ThenInitial(InitialParserTest test)
        {
            uint messagePart = this.makeInitialParser();
            test(messagePart);
        }

        private void ThenPartA(PartAParserTest test)
        {
            NmeaAisStaticDataReportParserPartA parser = this.makePartAParser();
            test(parser);
        }

        private void ThenPartB(PartBParserTest test)
        {
            NmeaAisStaticDataReportParserPartB parser = this.makePartBParser();
            test(parser);
        }
    }
}
