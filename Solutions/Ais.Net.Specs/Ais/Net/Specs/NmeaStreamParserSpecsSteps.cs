// <copyright file="NmeaStreamParserSpecsSteps.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Ais.Net.Specs
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Threading.Tasks;
    using NUnit.Framework;
    using TechTalk.SpecFlow;

    [Binding]
    public class NmeaStreamParserSpecsSteps
    {
        private readonly StringBuilder content = new StringBuilder();
        private readonly LineProcessor lineProcessor = new LineProcessor();
        private readonly NmeaAisMessageStreamProcessorBindings messageProcessor;

        public NmeaStreamParserSpecsSteps(NmeaAisMessageStreamProcessorBindings messageProcessor)
        {
            this.messageProcessor = messageProcessor;
        }

        [Given("no content")]
        public void GivenNoContent()
        {
            // Nothing to do here.
        }

        [Given("a line '(.*)'")]
        public void GivenALine(string line)
        {
            this.content.Append(line);
            this.content.Append("\n");
        }

        [Given("a CRLF line '(.*)'")]
        public void GivenACrlfLine(string line)
        {
            this.content.Append(line);
            this.content.Append("\r\n");
        }

        [Given("an unterminated line '(.*)'")]
        public void GivenAnUnterminatedLine(string line)
        {
            this.content.Append(line);
        }

        [When("I parse the content by line")]
        public async Task WhenIParseTheContentByLineAsync()
        {
            await NmeaStreamParser.ParseStreamAsync(
                new MemoryStream(Encoding.ASCII.GetBytes(this.content.ToString())),
                this.lineProcessor).ConfigureAwait(false);
        }

        [When("I parse the content by message")]
        public async Task WhenIParseTheContentByMessageAsync()
        {
            await NmeaStreamParser.ParseStreamAsync(
                new MemoryStream(Encoding.ASCII.GetBytes(this.content.ToString())),
                new NmeaLineToAisStreamAdapter(this.messageProcessor.Processor)).ConfigureAwait(false);
        }

        [When("I parse the content by message with exceptions disabled")]
        public async Task WhenIParseTheContentByMessageWithExceptionsDisabledAsync()
        {
            var options = new NmeaParserOptions { ThrowWhenTagBlockContainsUnknownFields = false };
            await NmeaStreamParser.ParseStreamAsync(
                new MemoryStream(Encoding.ASCII.GetBytes(this.content.ToString())),
                new NmeaLineToAisStreamAdapter(this.messageProcessor.Processor, options),
                options).ConfigureAwait(false);
        }

        [Then(@"INmeaLineStreamProcessor\.OnComplete should have been called")]
        public void ThenOnCompleteShouldHaveBeenCalled()
        {
            Assert.IsTrue(this.lineProcessor.IsComplete);
        }

        [Then(@"INmeaAisMessageStreamProcessor\.OnComplete should have been called")]
        public void ThenINmeaAisMessageStreamProcessor_OnCompleteShouldHaveBeenCalled()
        {
            Assert.IsTrue(this.messageProcessor.IsComplete);
        }

        [Then("INmeaLineStreamProcessor.OnNext should have been called (.*) times")]
        [Then("INmeaLineStreamProcessor.OnNext should have been called (.*) time")]
        public void ThenOnNextShouldHaveBeenCalledTimes(int count)
        {
            Assert.AreEqual(count, this.lineProcessor.OnNextCalls.Count);
        }

        [Then("OnError should have been called (.*) times")]
        [Then("OnError should have been called (.*) time")]
        public void ThenOnErrorShouldHaveBeenCalledTimes(int count)
        {
            Assert.AreEqual(count, this.lineProcessor.OnErrorCalls.Count);
        }

        [Then("line (.*) should have a tag block of '(.*)' and a sentence of '(.*)'")]
        public void ThenLineShouldHaveATagBlockOfAndASentenceOf(int line, string tagBlock, string sentence)
        {
            LineProcessor.Line call = this.lineProcessor.OnNextCalls[line];
            Assert.AreEqual(tagBlock, call.TagBlock);
            Assert.AreEqual(sentence, call.Sentence);
        }

        [Then("the line error report (.*) should include the problematic line '(.*)'")]
        public void ThenTheLineErrorReportShouldIncludeTheProblematicLine(int errorCallNumber, string line)
        {
            LineProcessor.ErrorReport call = this.lineProcessor.OnErrorCalls[errorCallNumber];
            Assert.AreEqual(line, call.Line);
        }

        [Then("the line error report (.*) should include an exception reporting that the expected exclamation mark is missing")]
        public void ThenTheLineErrorReportShouldIncludeAnExceptionReportingThatTheExpectedExclamationMarkIsMissing(int errorCallNumber)
        {
            LineProcessor.ErrorReport call = this.lineProcessor.OnErrorCalls[errorCallNumber];
            Assert.IsInstanceOf<ArgumentException>(call.Error);

            var e = (ArgumentException)call.Error;
            Assert.AreEqual("Invalid data. Expected '!' at sentence start", e.Message);
        }

        [Then("the message error report (.*) should include an exception reporting that the message appears to be incomplete")]
        public void ThenTheMessageErrorReportShouldIncludeAnExceptionReportingThatTheMessageAppearsToBeTruncated(int errorCallNumber)
        {
            NmeaAisMessageStreamProcessorBindings.ErrorReport call = this.messageProcessor.OnErrorCalls[errorCallNumber];
            Assert.IsInstanceOf<ArgumentException>(call.Error);

            var e = (ArgumentException)call.Error;
            Assert.AreEqual("Invalid data. The message appears to be missing some characters - it may have been corrupted or truncated.", e.Message);
        }

        [Then("the message error report (.*) should include an exception reporting that the padding is missing")]
        public void ThenTheMessageErrorReportShouldIncludeAnExceptionReportingThatThePaddingIsMissing(int errorCallNumber)
        {
            NmeaAisMessageStreamProcessorBindings.ErrorReport call = this.messageProcessor.OnErrorCalls[errorCallNumber];
            Assert.IsInstanceOf<ArgumentException>(call.Error);

            var e = (ArgumentException)call.Error;
            Assert.AreEqual("Invalid data. Payload padding field not present - the message may have been corrupted or truncated", e.Message);
        }

        [Then("the message error report (.*) should include an exception reporting that the checksum is missing")]
        public void ThenTheMessageErrorReportShouldIncludeAnExceptionReportingThatTheChecksumIsMissing(int errorCallNumber)
        {
            NmeaAisMessageStreamProcessorBindings.ErrorReport call = this.messageProcessor.OnErrorCalls[errorCallNumber];
            Assert.IsInstanceOf<ArgumentException>(call.Error);

            var e = (ArgumentException)call.Error;
            Assert.AreEqual("Invalid data. Payload checksum not present - the message may have been corrupted or truncated", e.Message);
        }

        [Then("the message error report (.*) should include an exception reporting that the expected exclamation mark is missing")]
        public void ThenTheMessageErrorReportShouldIncludeAnExceptionReportingThatTheExpectedExclamationMarkIsMissing(int errorCallNumber)
        {
            NmeaAisMessageStreamProcessorBindings.ErrorReport call = this.messageProcessor.OnErrorCalls[errorCallNumber];
            Assert.IsInstanceOf<ArgumentException>(call.Error);

            var e = (ArgumentException)call.Error;
            Assert.AreEqual("Invalid data. Expected '!' at sentence start", e.Message);
        }

        [Then("the message error report (.*) should include an exception reporting that an unrecognized field is present")]
        public void WhenTheMessageErrorReportShouldIncludeAnExceptionReportingThatAnUnrecognizedFieldIsPresent(int errorCallNumber)
        {
            NmeaAisMessageStreamProcessorBindings.ErrorReport call = this.messageProcessor.OnErrorCalls[errorCallNumber];
            Assert.IsInstanceOf<ArgumentException>(call.Error);

            var e = (ArgumentException)call.Error;
            const string expectedStart = "Unknown field type:";
            Assert.AreEqual(expectedStart, e.Message.Substring(0, expectedStart.Length));
        }

        [Then("the message error report (.*) should include an exception reporting that an unsupported field is present")]
        public void ThenTheMessageErrorReportShouldIncludeAnExceptionReportingThatAnUnsupportedFieldIsPresent(int errorCallNumber)
        {
            NmeaAisMessageStreamProcessorBindings.ErrorReport call = this.messageProcessor.OnErrorCalls[errorCallNumber];
            Assert.IsInstanceOf<NotSupportedException>(call.Error);

            var e = (NotSupportedException)call.Error;
            const string expectedStart = "Unsupported field type:";
            Assert.AreEqual(expectedStart, e.Message.Substring(0, expectedStart.Length));
        }

        [Then("the line error report (.*) should include the line number (.*)")]
        public void ThenTheLineErrorReportShouldIncludeTheLineNumber(int errorCallNumber, int lineNumber)
        {
            LineProcessor.ErrorReport call = this.lineProcessor.OnErrorCalls[errorCallNumber];
            Assert.AreEqual(lineNumber, call.LineNumber);
        }

        private class LineProcessor : INmeaLineStreamProcessor
        {
            public bool IsComplete { get; private set; }

            public List<Line> OnNextCalls { get; } = new List<Line>();

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

            public void OnNext(in NmeaLineParser value, int lineNumber)
            {
                if (this.IsComplete)
                {
                    throw new InvalidOperationException($"Must not call {nameof(this.OnNext)} after calling {nameof(this.OnCompleted)}");
                }

                this.OnNextCalls.Add(new Line(
                    Encoding.ASCII.GetString(value.TagBlockAsciiWithoutDelimiters),
                    Encoding.ASCII.GetString(value.Sentence)));
            }

            public void Progress(bool done, int totalLines, int totalTicks, int linesSinceLastUpdate, int ticksSinceLastUpdate)
            {
            }

            public class Line
            {
                public Line(string tagBlock, string sentence)
                {
                    this.TagBlock = tagBlock;
                    this.Sentence = sentence;
                }

                public string TagBlock { get; }

                public string Sentence { get; }
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