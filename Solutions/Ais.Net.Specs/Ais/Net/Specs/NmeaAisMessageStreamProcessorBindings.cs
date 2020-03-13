// <copyright file="NmeaLineToAisStreamAdapterSpecsSteps.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Ais.Net.Specs
{
    using System;
    using System.Buffers.Text;
    using System.Collections.Generic;
    using System.Text;
    using NUnit.Framework;
    using TechTalk.SpecFlow;

    [Binding]
    public class NmeaAisMessageStreamProcessorBindings
    {
        private readonly MessageProcessor processor;

        public NmeaAisMessageStreamProcessorBindings()
        {
            this.processor = new MessageProcessor(this);
        }

        public INmeaAisMessageStreamProcessor Processor => this.processor;

        public List<Message> OnNextCalls { get; } = new List<Message>();

        public List<ProgressReport> ProgressCalls { get; } = new List<ProgressReport>();

        public List<ErrorReport> OnErrorCalls { get; } = new List<ErrorReport>();

        public bool IsComplete { get; private set; }

        [Then("INmeaAisMessageStreamProcessor.OnNext should have been called (.*) time")]
        [Then("INmeaAisMessageStreamProcessor.OnNext should have been called (.*) times")]
        public void ThenTheAisMessageProcessorShouldReceiveMessages(int messageCount)
        {
            Assert.AreEqual(messageCount, this.OnNextCalls.Count);
        }

        [Then("in ais message (.*) the payload should be '(.*)' with padding of (.*)")]
        public void ThenAisPayloadShouldBeWithPaddingOf(int callIndex, string payloadAscii, int padding)
        {
            Assert.AreEqual(payloadAscii, this.OnNextCalls[callIndex].AsciiPayload);
            Assert.AreEqual(padding, this.OnNextCalls[callIndex].Padding);
        }

        [Then("in ais message (.*) the source from the first NMEA line should be (.*)")]
        public void ThenInAisMessageTheSourceFromTheFirstNMEALineShouldBe(int callIndex, int source)
        {
            Assert.AreEqual(source, this.OnNextCalls[callIndex].Source);
        }

        [Then("in ais message (.*) the timestamp from the first NMEA line should be (.*)")]
        public void ThenInAisMessageTheTimestampFromTheFirstNMEALineShouldBe(int callIndex, int timestamp)
        {
            Assert.AreEqual(timestamp, this.OnNextCalls[callIndex].UnixTimestamp);
        }

        [Then("INmeaAisMessageStreamProcessor.Progress should have been called (.*) times")]
        public void ThenTheAisMessageProcessorShouldReceiveProgressReports(int callCount)
        {
            Assert.AreEqual(callCount, this.ProgressCalls.Count);
        }

        [Then("INmeaAisMessageStreamProcessor.OnError should have been called (.*) times")]
        [Then("INmeaAisMessageStreamProcessor.OnError should have been called (.*) time")]
        public void ThenTheAisMessageProcessorShouldReceiveAnErrorReport(int errorCount)
        {
            Assert.AreEqual(errorCount, this.OnErrorCalls.Count);
        }

        [Then("the message error report (.*) should include the problematic line '(.*)'")]
        public void ThenTheMessageErrorReportShouldIncludeTheProblematicLine(int errorCallNumber, string line)
        {
            NmeaAisMessageStreamProcessorBindings.ErrorReport call = this.OnErrorCalls[errorCallNumber];
            Assert.AreEqual(line, call.Line);
        }

        [Then("the message error report (.*) should include an exception reporting unexpected padding on a non-terminal message fragment")]
        public void ThenTheMessageErrorReportShouldIncludeAnExceptionReportingUnexpectedPaddingOnANon_TerminalMessageFragment(int errorCallNumber)
        {
            NmeaAisMessageStreamProcessorBindings.ErrorReport call = this.OnErrorCalls[errorCallNumber];
            Assert.IsInstanceOf<ArgumentException>(call.Error);

            var e = (ArgumentException)call.Error;
            Assert.AreEqual("Can only handle non-zero padding on the final message in a fragment", e.Message);
        }

        [Then("the message error report (.*) should include an exception reporting that it has received two message fragments with the same group id and position")]
        public void ThenTheMessageErrorReportShouldIncludeAnExceptionReportingThatItHasReceivedTwoMessageFragmentsWithTheSameGroupIdAndPosition(int errorCallNumber)
        {
            NmeaAisMessageStreamProcessorBindings.ErrorReport call = this.OnErrorCalls[errorCallNumber];
            Assert.IsInstanceOf<ArgumentException>(call.Error);

            var e = (ArgumentException)call.Error;
            const string expectedStart = "Already received sentence ";
            Assert.AreEqual(expectedStart, e.Message.Substring(0, expectedStart.Length));
        }

        [Then("the message error report (.*) should include an exception reporting that it received an incomplete set of fragments for a message")]
        public void ThenTheMessageErrorReportShouldIncludeAnExceptionReportingThatItReceivedAnIncompleteSetOfFragmentsForAMessage(int errorCallNumber)
        {
            NmeaAisMessageStreamProcessorBindings.ErrorReport call = this.OnErrorCalls[errorCallNumber];
            Assert.IsInstanceOf<ArgumentException>(call.Error);

            var e = (ArgumentException)call.Error;
            Assert.AreEqual("Received incomplete fragmented message.", e.Message);
        }

        [Then("the message error report (.*) should include the line number (.*)")]
        public void ThenTheMessageErrorReportShouldIncludeTheLineNumber(int errorCallNumber, int lineNumber)
        {
            NmeaAisMessageStreamProcessorBindings.ErrorReport call = this.OnErrorCalls[errorCallNumber];
            Assert.AreEqual(lineNumber, call.LineNumber);
        }

        [Then("progress report (.*) was (.*), (.*), (.*), (.*), (.*), (.*), (.*)")]
        public void ThenProgressReportWasFalse(
            int callIndex,
            bool done,
            int totalNmeaLines,
            int totalAisMessages,
            int totalTicks,
            int nmeaLinesSinceLastUpdate,
            int aisMessagesSinceLastUpdate,
            int ticksSinceLastUpdate)
        {
            NmeaAisMessageStreamProcessorBindings.ProgressReport call = this.ProgressCalls[callIndex];
            Assert.AreEqual(done, call.Done);
            Assert.AreEqual(totalNmeaLines, call.TotalNmeaLines);
            Assert.AreEqual(totalAisMessages, call.TotalAisMessages);
            Assert.AreEqual(totalTicks, call.TotalTicks);
            Assert.AreEqual(nmeaLinesSinceLastUpdate, call.NmeaLinesSinceLastUpdate);
            Assert.AreEqual(aisMessagesSinceLastUpdate, call.AisMessagesSinceLastUpdate);
            Assert.AreEqual(ticksSinceLastUpdate, call.TicksSinceLastUpdate);
        }

        public class Message
        {
            public Message(
                long? unixTimestamp,
                int source,
                string asciiPayload,
                uint padding)
            {
                this.UnixTimestamp = unixTimestamp;
                this.Source = source;
                this.AsciiPayload = asciiPayload;
                this.Padding = padding;
            }

            public long? UnixTimestamp { get; }

            public int Source { get; }

            public string AsciiPayload { get; }

            public uint Padding { get; }
        }

        public class ProgressReport
        {
            public ProgressReport(
                bool done,
                int totalNmeaLines,
                int totalAisMessages,
                int totalTicks,
                int nmeaLinesSinceLastUpdate,
                int aisMessagesSinceLastUpdate,
                int ticksSinceLastUpdate)
            {
                this.Done = done;
                this.TotalNmeaLines = totalNmeaLines;
                this.TotalAisMessages = totalAisMessages;
                this.TotalTicks = totalTicks;
                this.TicksSinceLastUpdate = ticksSinceLastUpdate;
                this.NmeaLinesSinceLastUpdate = nmeaLinesSinceLastUpdate;
                this.AisMessagesSinceLastUpdate = aisMessagesSinceLastUpdate;
            }

            public bool Done { get; }

            public int TotalNmeaLines { get; }

            public int TotalAisMessages { get; }

            public int TotalTicks { get; }

            public int LinesSinceLastUpdate { get; }

            public int TicksSinceLastUpdate { get; }

            public int NmeaLinesSinceLastUpdate { get; }

            public int AisMessagesSinceLastUpdate { get; }
        }

        public class ErrorReport
        {
            public ErrorReport(string line, Exception error, int lineNumber)
            {
                this.Line = line;
                this.Error = error;
                this.LineNumber = lineNumber;
            }

            public string Line { get; }

            public Exception Error { get; }

            public int LineNumber { get; }
        }

        private class MessageProcessor : INmeaAisMessageStreamProcessor
        {
            private readonly NmeaAisMessageStreamProcessorBindings parent;

            public MessageProcessor(NmeaAisMessageStreamProcessorBindings nmeaAisMessageStreamProcessorBindings)
            {
                this.parent = nmeaAisMessageStreamProcessorBindings;
            }

            public void OnCompleted()
            {
                if (this.parent.IsComplete)
                {
                    throw new InvalidOperationException($"Must not call {nameof(this.OnCompleted)} more than once");
                }

                this.parent.IsComplete = true;
            }

            public void OnError(in ReadOnlySpan<byte> line, Exception error, int lineNumber)
            {
                this.parent.OnErrorCalls.Add(new ErrorReport(Encoding.ASCII.GetString(line), error, lineNumber));
            }

            public void OnNext(
                in NmeaLineParser firstLine,
                in ReadOnlySpan<byte> asciiPayload,
                uint padding)
            {
                if (this.parent.IsComplete)
                {
                    throw new InvalidOperationException($"Must not call {nameof(this.OnNext)} after calling {nameof(this.OnCompleted)}");
                }

                this.parent.OnNextCalls.Add(new Message(
                    firstLine.TagBlock.UnixTimestamp,
                    Utf8Parser.TryParse(firstLine.TagBlock.Source, out int sourceId, out _) ? sourceId : throw new ArgumentException("Test must supply valid source"),
                    Encoding.ASCII.GetString(asciiPayload),
                    padding));
            }

            public void Progress(
                bool done,
                int totalNmeaLines,
                int totalAisMessages,
                int totalTicks,
                int nmeaLinesSinceLastUpdate,
                int aisMessagesSinceLastUpdate,
                int ticksSinceLastUpdate)
            {
                this.parent.ProgressCalls.Add(new ProgressReport(done, totalNmeaLines, totalAisMessages, totalTicks, nmeaLinesSinceLastUpdate, aisMessagesSinceLastUpdate, ticksSinceLastUpdate));
            }
        }
    }
}