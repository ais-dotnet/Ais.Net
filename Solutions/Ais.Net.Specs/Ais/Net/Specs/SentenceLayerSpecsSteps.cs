// <copyright file="SentenceLayerSpecsSteps.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Ais.Net.Specs
{
    using System.Text;
    using NUnit.Framework;
    using TechTalk.SpecFlow;

    [Binding]
    public class SentenceLayerSpecsSteps
    {
        private ParserMaker makeParser;

        private delegate NmeaLineParser ParserMaker();

        private delegate void ParserTest(NmeaLineParser parser);

        [When("I parse a message with no tag block")]
        public void WhenIParseAMessageWithNoTagBlock()
        {
            this.When(AivdmExamples.SomeSortOfAivdmMessageRenameThisWhenWeKnowMore);
        }

        [When("I parse a message with a tag block")]
        public void WhenIParseAMessageWithATagBlock()
        {
            this.When(AivdmExamples.SomeSortOfAivdmMessageRenameThisWhenWeKnowMoreWithTagBlock);
        }

        [When("I parse a message with a packet tag field of '(.*)'")]
        public void WhenIParseAMessageWithAPacketTagFieldOf(string tag)
        {
            this.When(string.Format(AivdmExamples.MessageWithTaAGPLaceholderFormat, tag));
        }

        [When("I parse a message fragment part (.*) of (.*) with message id (.*) and sentence group id (.*)")]
        public void WhenIParseAMessageFragmentPartOfWithMessageIdAndSentenceGroupId(
            int currentFragment, int totalFragments, string sequentialMessageId, string sentenceGroupId)
        {
            this.When(string.Format(AivdmExamples.MessageFragmentFormat, currentFragment, totalFragments, sequentialMessageId, sentenceGroupId));
        }

        [When("I parse a message fragment part (.*) of (.*) with message id (.*) and no sentence group id")]
        public void WhenIParseAMessageFragmentPartOfWithMessageIdAndNoSentenceGroupId(
            int currentFragment, int totalFragments, string sequentialMessageId)
        {
            this.When(string.Format(AivdmExamples.MessageFragmentWithoutGroupInHeaderFormat, currentFragment, totalFragments, sequentialMessageId));
        }

        [When("I parse a non-fragmented message")]
        public void WhenIParseANon_FragmentedMessage()
        {
            this.When(AivdmExamples.NonFragmentedMessage);
        }

        [When("I parse a message with a radio channel code of '(.*)'")]
        public void WhenIParseAMessageWithARadioChannelCodeOf(string channel)
        {
            this.When(string.Format(AivdmExamples.MessageWithRadioChannelPlaceholderFormat, channel));
        }

        [When("I parse a message with a payload of '(.*)'")]
        public void WhenIParseAMessageWithAPayloadOf(string payload)
        {
            this.When(string.Format(AivdmExamples.MessageWithPayloadPlaceholderFormat, payload));
        }

        [When("I parse a message with padding of (.*)")]
        public void WhenIParseAMessageWithAPaddingOf(int padding)
        {
            this.When(string.Format(AivdmExamples.MessageWithPaddinAGPLaceholderFormat, padding));
        }

        [Then("the TagBlockWithoutDelimiters property's Length should be (.*)")]
        public void ThenTheTagBlockWithoutDelimitersLengthShouldBe(int expectedLength)
        {
            this.Then(parser => Assert.AreEqual(expectedLength, parser.TagBlockAsciiWithoutDelimiters.Length));
        }

        [Then("the TagBlockWithoutDelimiters property should match the tag block without the delimiters")]
        public void ThenTheTagBlockWithoutDelimitersPropertyShouldMatchTheTagBlockWithoutTheDelimiters()
        {
            this.Then(parser =>
            {
                string parsedTagBlock = Encoding.ASCII.GetString(parser.TagBlockAsciiWithoutDelimiters);
                Assert.AreEqual(AivdmExamples.SimpleTagBlockWithoutDelimiters, parsedTagBlock);
            });
        }

        [Then("the AisTalker is '(.*)'")]
        public void ThenTheAisTalkerIs(TalkerId talkerId)
        {
            this.Then(parser => Assert.AreEqual(talkerId, parser.AisTalker));
        }

        [Then("the DataOrigin is '(.*)'")]
        public void ThenTheDataOriginIs(VesselDataOrigin dataOrigin)
        {
            this.Then(parser => Assert.AreEqual(dataOrigin, parser.DataOrigin));
        }

        [Then("the TotalFragmentCount is '(.*)'")]
        public void ThenTheTotalFragmentCountIs(int totalFragments)
        {
            this.Then(parser => Assert.AreEqual(totalFragments, parser.TotalFragmentCount));
        }

        [Then("the FragmentNumberOneBased is '(.*)'")]
        public void ThenTheFragmentNumberOneBasedIs(int currentFragment)
        {
            this.Then(parser => Assert.AreEqual(currentFragment, parser.FragmentNumberOneBased));
        }

        [Then("the MultiSequenceMessageId is '(.*)'")]
        public void ThenTheMultiSequenceMessageIdIs(string sequentialMessageId)
        {
            this.Then(parser =>
            {
                string parsedMessageId = Encoding.ASCII.GetString(parser.MultiSequenceMessageId);
                Assert.AreEqual(sequentialMessageId, parsedMessageId);
            });
        }

        [Then("the MultiSequenceMessageId is empty")]
        public void ThenTheMultiSequenceMessageIdIsEmpty()
        {
            this.Then(parser => Assert.IsTrue(parser.MultiSequenceMessageId.IsEmpty));
        }

        [Then("the TagBlockSentenceGrouping is not present")]
        public void ThenTheTagBlockSentenceGroupingIsNotPresent()
        {
            this.Then(parser => Assert.IsFalse(parser.TagBlock.SentenceGrouping.HasValue));
        }

        [Then("the SentenceGroupId is '(.*)'")]
        public void ThenTheSentenceGroupIdIs(int sentenceGroupId)
        {
            this.Then(parser => Assert.AreEqual(sentenceGroupId, parser.TagBlock.SentenceGrouping.Value.GroupId));
        }

        [Then("the ChannelCode is '(.*)'")]
        public void ThenTheChannelCodeIs(char channelCode)
        {
            this.Then(parser => Assert.AreEqual(channelCode, parser.ChannelCode));
        }

        [Then("the payload is '(.*)'")]
        public void ThenThePayloadIs(string payload)
        {
            this.Then(parser =>
            {
                string parsedPayload = Encoding.ASCII.GetString(parser.Payload);
                Assert.AreEqual(payload, parsedPayload);
            });
        }

        [Then("the padding is (.*)")]
        public void ThenThePaddingIs(int padding)
        {
            this.Then(parser => Assert.AreEqual(padding, parser.Padding));
        }

        private void When(string messageLine)
        {
            this.When(() => new NmeaLineParser(Encoding.ASCII.GetBytes(messageLine)));
        }

        private void When(ParserMaker makeParser)
        {
            this.makeParser = makeParser;
        }

        private void Then(ParserTest test)
        {
            NmeaLineParser parser = this.makeParser();
            test(parser);
        }
    }
}