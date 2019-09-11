// <copyright file="NmeaAisBitVectorParserSpecsSteps.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Ais.Net.Specs
{
    using System.Text;
    using NUnit.Framework;
    using TechTalk.SpecFlow;

    [Binding]
    public class NmeaAisBitVectorParserSpecsSteps
    {
        private ParserMaker makeParser;
        private uint unsignedIntegerResult;
        private int signedIntegerResult;

        private delegate NmeaAisBitVectorParser ParserMaker();

        private delegate void ParserTest(NmeaAisBitVectorParser parser);

        [Given("an NMEA AIS payload of '(.*)' and padding (.*)")]
        public void GivenAnNMEAAISPayloadOfAndPadding(string payload, uint padding)
        {
            this.Given(() => new NmeaAisBitVectorParser(Encoding.ASCII.GetBytes(payload), padding));
        }

        [When("I read an unsigned (.*) bit int at offset (.*)")]
        public void WhenIReadAnUnsignedBitIntAtOffset(uint bitCount, uint offset)
        {
            this.When(p => this.unsignedIntegerResult = p.GetUnsignedInteger(bitCount, offset));
        }

        [When("I read a signed (.*) bit int at offset (.*)")]
        public void WhenIReadASignedBitIntAtOffset(uint bitCount, uint offset)
        {
            this.When(p => this.signedIntegerResult = p.GetSignedInteger(bitCount, offset));
        }

        [Then("the NmeaAisBitVectorParser returns an unsigned integer with value (.*)")]
        public void ThenTheNmeaAisBitVectorParserReturnsAnUnsignedIntegerWithValue(int expectedValue)
        {
            Assert.AreEqual(expectedValue, this.unsignedIntegerResult);
        }

        [Then("the NmeaAisBitVectorParser returns an signed integer with value (.*)")]
        public void ThenTheNmeaAisBitVectorParserReturnsAnSignedIntegerWithValue(int expectedValue)
        {
            Assert.AreEqual(expectedValue, this.signedIntegerResult);
        }

        private void Given(ParserMaker makeParser)
        {
            this.makeParser = makeParser;
        }

        private void When(ParserTest test)
        {
            NmeaAisBitVectorParser parser = this.makeParser();
            test(parser);
        }
    }
}