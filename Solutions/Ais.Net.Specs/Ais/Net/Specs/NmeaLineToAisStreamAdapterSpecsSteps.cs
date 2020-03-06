// <copyright file="NmeaLineToAisStreamAdapterSpecsSteps.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Ais.Net.Specs
{
    using System;
    using System.Text;
    using NUnit.Framework;
    using TechTalk.SpecFlow;

    [Binding]
    public class NmeaLineToAisStreamAdapterSpecsSteps : IDisposable
    {
        private readonly NmeaLineToAisStreamAdapter adapter;
        private readonly NmeaAisMessageStreamProcessorBindings processor;
        private bool adapterOnCompleteCalled = false;
        private Exception exceptionProvidedToProcessor;
        private int lineNumber = 1;

        public NmeaLineToAisStreamAdapterSpecsSteps(NmeaAisMessageStreamProcessorBindings processorBindings)
        {
            this.adapter = new NmeaLineToAisStreamAdapter(processorBindings.Processor);
            this.processor = processorBindings;
        }

        [Then("the message error report (.*) should include the exception reported by the line stream parser")]
        public void ThenTheMessageErrorReportShouldIncludeTheExceptionReportedByTheLineStreamParser(int errorCallNumber)
        {
            NmeaAisMessageStreamProcessorBindings.ErrorReport call = this.processor.OnErrorCalls[errorCallNumber];
            Assert.AreSame(this.exceptionProvidedToProcessor, call.Error);
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
    }
}