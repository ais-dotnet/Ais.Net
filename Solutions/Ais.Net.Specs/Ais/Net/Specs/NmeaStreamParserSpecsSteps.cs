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
        private readonly Processor processor = new Processor();

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

        [When("I parse the content")]
        public async Task WhenIParseTheContentAsync()
        {
            await NmeaStreamParser.ParseStreamAsync(
                new MemoryStream(Encoding.ASCII.GetBytes(this.content.ToString())),
                this.processor).ConfigureAwait(false);
        }

        [Then("OnComplete should have been called")]
        public void ThenOnCompleteShouldHaveBeenCalled()
        {
            Assert.IsTrue(this.processor.IsComplete);
        }

        [Then("OnNext should have been called (.*) times")]
        public void ThenOnNextShouldHaveBeenCalledTimes(int count)
        {
            Assert.AreEqual(count, this.processor.OnNextCalls.Count);
        }

        [Then("OnError should have been called (.*) times")]
        public void ThenOnErrorShouldHaveBeenCalledTimes(int count)
        {
            Assert.AreEqual(count, this.processor.OnErrorCalls.Count);
        }

        [Then("line (.*) should have a tag block of '(.*)' and a sentence of '(.*)'")]
        public void ThenLineShouldHaveATagBlockOfAndASentenceOf(int line, string tagBlock, string sentence)
        {
            Processor.Line call = this.processor.OnNextCalls[line];
            Assert.AreEqual(tagBlock, call.TagBlock);
            Assert.AreEqual(sentence, call.Sentence);
        }

        [Then("the line error report (.*) should include the problematic line '(.*)'")]
        public void ThenTheLineErrorReportShouldIncludeTheProblematicLine(int errorCallNumber, string line)
        {
            Processor.ErrorReport call = this.processor.OnErrorCalls[errorCallNumber];
            Assert.AreEqual(line, call.Line);
        }

        [Then("the line error report (.*) should include an exception reporting that the expected exclamation mark is missing")]
        public void ThenTheLineErrorReportShouldIncludeAnExceptionReportingThatTheExpectedExclamationMarkIsMissing(int errorCallNumber)
        {
            Processor.ErrorReport call = this.processor.OnErrorCalls[errorCallNumber];
            Assert.IsInstanceOf<ArgumentException>(call.Error);

            var e = (ArgumentException)call.Error;
            Assert.AreEqual("Invalid data. Expected '!' at sentence start", e.Message);
        }

        [Then("the line error report (.*) should include the line number (.*)")]
        public void ThenTheLineErrorReportShouldIncludeTheLineNumber(int errorCallNumber, int lineNumber)
        {
            Processor.ErrorReport call = this.processor.OnErrorCalls[errorCallNumber];
            Assert.AreEqual(lineNumber, call.LineNumber);
        }

        private class Processor : INmeaLineStreamProcessor
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