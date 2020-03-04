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
    public class NmeaLineToAisStreamAdapterSpecsSteps : IDisposable
    {
        private readonly Processor processor = new Processor();
        private readonly NmeaLineToAisStreamAdapter adapter;
        private bool adapterOnCompleteCalled = false;
        private Exception exceptionProvidedToProcessor;
        private int lineNumber = 1;

        public NmeaLineToAisStreamAdapterSpecsSteps()
        {
            this.adapter = new NmeaLineToAisStreamAdapter(this.processor);
        }

        public void Dispose()
        {
            if (!this.adapterOnCompleteCalled)
            {
                this.adapter.OnCompleted();
                this.adapterOnCompleteCalled = true;
            }

            this.adapter.Dispose();
        }

        [When("the line to message adapter receives '(.*)'")]
        public void WhenTheLineToMessageAdapterReceives(string line)
        {
            byte[] ascii = Encoding.ASCII.GetBytes(line);
            var lineParser = new NmeaLineParser(ascii);
            this.adapter.OnNext(lineParser, this.lineNumber++);
        }

        [When("the line to message adapter receives an error report for content '(.*)' with line number (.*)")]
        public void WhenTheLineToMessageAdapterReceivesAnErrorReportForContentWithLineNumber(string line, int lineNumber)
        {
            byte[] ascii = Encoding.ASCII.GetBytes(line);
            this.exceptionProvidedToProcessor = new ArgumentException("That was never 5 minutes");
            this.adapter.OnError(ascii, this.exceptionProvidedToProcessor, lineNumber);
        }

        [When("the line to message adapter receives a progress report of (.*), (.*), (.*), (.*), (.*)")]
        public void WhenTheLineToMessageAdapterReceivesAProgressReportOfFalse(
            bool done, int totalLines, int totalTicks, int linesSinceLastUpdate, int ticksSinceLastUpdate)
        {
            this.adapter.Progress(done, totalLines, totalTicks, linesSinceLastUpdate, ticksSinceLastUpdate);
        }

        [Then("the ais message processor should receive (.*) message")]
        [Then("the ais message processor should receive (.*) messages")]
        public void ThenTheAisMessageProcessorShouldReceiveMessages(int messageCount)
        {
            Assert.AreEqual(messageCount, this.processor.OnNextCalls.Count);
        }

        [Then("in ais message (.*) the payload should be '(.*)' with padding of (.*)")]
        public void ThenAisPayloadShouldBeWithPaddingOf(int callIndex, string payloadAscii, int padding)
        {
            Assert.AreEqual(payloadAscii, this.processor.OnNextCalls[callIndex].AsciiPayload);
            Assert.AreEqual(padding, this.processor.OnNextCalls[callIndex].Padding);
        }

        [Then("in ais message (.*) the source from the first NMEA line should be (.*)")]
        public void ThenInAisMessageTheSourceFromTheFirstNMEALineShouldBe(int callIndex, int source)
        {
            Assert.AreEqual(source, this.processor.OnNextCalls[callIndex].Source);
        }

        [Then("in ais message (.*) the timestamp from the first NMEA line should be (.*)")]
        public void ThenInAisMessageTheTimestampFromTheFirstNMEALineShouldBe(int callIndex, int timestamp)
        {
            Assert.AreEqual(timestamp, this.processor.OnNextCalls[callIndex].UnixTimestamp);
        }

        [Then("the ais message processor should receive (.*) progress reports")]
        public void ThenTheAisMessageProcessorShouldReceiveProgressReports(int callCount)
        {
            Assert.AreEqual(callCount, this.processor.ProgressCalls.Count);
        }

        [Then("the ais message processor should receive (.*) error reports")]
        public void ThenTheAisMessageProcessorShouldReceiveAnErrorReport(int errorCount)
        {
            Assert.AreEqual(errorCount, this.processor.OnErrorCalls.Count);
        }

        [Then("the message error report (.*) should include the problematic line '(.*)'")]
        public void ThenTheMessageErrorReportShouldIncludeTheProblematicLine(int errorCallNumber, string line)
        {
            Processor.ErrorReport call = this.processor.OnErrorCalls[errorCallNumber];
            Assert.AreEqual(line, call.Line);
        }

        [Then("the message error report (.*) should include the exception reported by the line stream parser")]
        public void ThenTheMessageErrorReportShouldIncludeTheExceptionReportedByTheLineStreamParser(int errorCallNumber)
        {
            Processor.ErrorReport call = this.processor.OnErrorCalls[errorCallNumber];
            Assert.AreSame(this.exceptionProvidedToProcessor, call.Error);
        }

        [Then("the message error report (.*) should include an exception reporting unexpected padding on a non-terminal message fragment")]
        public void ThenTheMessageErrorReportShouldIncludeAnExceptionReportingUnexpectedPaddingOnANon_TerminalMessageFragment(int errorCallNumber)
        {
            Processor.ErrorReport call = this.processor.OnErrorCalls[errorCallNumber];
            Assert.IsInstanceOf<ArgumentException>(call.Error);

            var e = (ArgumentException)call.Error;
            Assert.AreEqual("Can only handle non-zero padding on the final message in a fragment", e.Message);
        }

        [Then(@"the message error report (.*) should include an exception reporting that it has received two message fragments with the same group id and position")]
        public void ThenTheMessageErrorReportShouldIncludeAnExceptionReportingThatItHasReceivedTwoMessageFragmentsWithTheSameGroupIdAndPosition(int errorCallNumber)
        {
            Processor.ErrorReport call = this.processor.OnErrorCalls[errorCallNumber];
            Assert.IsInstanceOf<ArgumentException>(call.Error);

            var e = (ArgumentException)call.Error;
            string expectedStart = "Already received sentence ";
            Assert.AreEqual(expectedStart, e.Message.Substring(0, expectedStart.Length));
        }

        [Then("the message error report (.*) should include the line number (.*)")]
        public void ThenTheMessageErrorReportShouldIncludeTheLineNumber(int errorCallNumber, int lineNumber)
        {
            Processor.ErrorReport call = this.processor.OnErrorCalls[errorCallNumber];
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
            Processor.ProgressReport call = this.processor.ProgressCalls[callIndex];
            Assert.AreEqual(done, call.Done);
            Assert.AreEqual(totalNmeaLines, call.TotalNmeaLines);
            Assert.AreEqual(totalAisMessages, call.TotalAisMessages);
            Assert.AreEqual(totalTicks, call.TotalTicks);
            Assert.AreEqual(nmeaLinesSinceLastUpdate, call.NmeaLinesSinceLastUpdate);
            Assert.AreEqual(aisMessagesSinceLastUpdate, call.AisMessagesSinceLastUpdate);
            Assert.AreEqual(ticksSinceLastUpdate, call.TicksSinceLastUpdate);
        }

        private class Processor : INmeaAisMessageStreamProcessor
        {
            public bool IsComplete { get; private set; }

            public List<Message> OnNextCalls { get; } = new List<Message>();

            public List<ProgressReport> ProgressCalls { get; } = new List<ProgressReport>();

            public List<ErrorReport> OnErrorCalls { get; } = new List<ErrorReport>();

            public void OnCompleted()
            {
                if (this.IsComplete)
                {
                    throw new InvalidOperationException($"Must not call {nameof(this.OnCompleted)} more than once");
                }

                this.IsComplete = true;
            }

            public void OnError(in ReadOnlySpan<byte> line, Exception error, int lineNumber)
            {
                this.OnErrorCalls.Add(new ErrorReport(Encoding.ASCII.GetString(line), error, lineNumber));
            }

            public void OnNext(
                in NmeaLineParser firstLine,
                in ReadOnlySpan<byte> asciiPayload,
                uint padding)
            {
                if (this.IsComplete)
                {
                    throw new InvalidOperationException($"Must not call {nameof(this.OnNext)} after calling {nameof(this.OnCompleted)}");
                }

                this.OnNextCalls.Add(new Message(
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
                this.ProgressCalls.Add(new ProgressReport(done, totalNmeaLines, totalAisMessages, totalTicks, nmeaLinesSinceLastUpdate, aisMessagesSinceLastUpdate, ticksSinceLastUpdate));
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
        }
    }
}