// <copyright file="ParsePayloadSpecsSteps.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Ais.Net.Specs
{
    using System.Text;
    using NUnit.Framework;
    using TechTalk.SpecFlow;

    [Binding]
    public class ParsePayloadSpecsSteps
    {
        private int peekedType;

        [When("I peek at the payload '(.*)' with padding of (.*)")]
        public void WhenIPeekAtThePayloadWithPaddingOf(string payload, uint padding)
        {
            this.peekedType = NmeaPayloadParser.PeekMessageType(Encoding.ASCII.GetBytes(payload), padding);
        }

        [Then("the message type returned by peek should be (.*)")]
        public void ThenTheMessageTypeReturnedByPeekShouldBe(int type)
        {
            Assert.AreEqual(type, this.peekedType);
        }
    }
}