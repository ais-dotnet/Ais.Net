// <copyright file="AisStringsSpecsSteps.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Ais.Net.Specs
{
    using Ais.Net;
    using NUnit.Framework;
    using TechTalk.SpecFlow;

    [Binding]
    public class AisStringsSpecsSteps
    {
        private byte asciiValue;

        public static void TestString(string expected, int fieldSizeInChars, in NmeaAisTextFieldParser parser)
        {
            // Although text fields are supposed to be padded with '@' characters, it's common
            // for real transponders to be set up to use spaces instead. And since SpecFlow
            // doesn't make is straightforward to include a space at the start or end of a test
            // string, we should pad out with spaces by default, because tests can explicitly
            // pad with @ in cases where that's what's expected.
            expected = expected.PadRight(fieldSizeInChars, ' ');
            for (int i = 0; i < expected.Length; ++i)
            {
                byte aisCharValue = parser.GetAscii((uint)i);
                Assert.AreEqual(expected[i], (char)aisCharValue);
            }
        }

        [When("I convert the AIS character value (.*) to ASCII")]
        public void WhenIConvertTheAISCharacterValueToASCII(byte aisChar)
        {
            this.asciiValue = AisStrings.AisCharacterToAsciiValue(aisChar);
        }

        [Then("the converted ASCII value should be '(.*)'")]
        public void ThenTheConvertedASCIIValueShouldBe(char c)
        {
            Assert.AreEqual(c, (char)this.asciiValue);
        }
    }
}