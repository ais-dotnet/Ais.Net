// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (http://www.specflow.org/).
//      SpecFlow Version:3.1.0.0
//      SpecFlow Generator Version:3.1.0.0
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
#pragma warning disable
namespace Ais.Net.Specs.AisMessageTypes
{
    using TechTalk.SpecFlow;
    using System;
    using System.Linq;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "3.1.0.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [NUnit.Framework.TestFixtureAttribute()]
    [NUnit.Framework.DescriptionAttribute("LongRangeAisBroadcastParserSpecs")]
    public partial class LongRangeAisBroadcastParserSpecsFeature
    {
        
        private TechTalk.SpecFlow.ITestRunner testRunner;
        
        private string[] _featureTags = ((string[])(null));
        
#line 1 "LongRangeAisBroadcastParserSpecs.feature"
#line hidden
        
        [NUnit.Framework.OneTimeSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "LongRangeAisBroadcastParserSpecs", "    In order process AIS messages from an nm4 file\r\n    As a developer\r\n    I wan" +
                    "t the NmeaAisLongRangeAisBroadcastParser to be able to parse the payload section" +
                    " of message type 27: Long Range AIS Broadcast", ProgrammingLanguage.CSharp, ((string[])(null)));
            testRunner.OnFeatureStart(featureInfo);
        }
        
        [NUnit.Framework.OneTimeTearDownAttribute()]
        public virtual void FeatureTearDown()
        {
            testRunner.OnFeatureEnd();
            testRunner = null;
        }
        
        [NUnit.Framework.SetUpAttribute()]
        public virtual void TestInitialize()
        {
        }
        
        [NUnit.Framework.TearDownAttribute()]
        public virtual void TestTearDown()
        {
            testRunner.OnScenarioEnd();
        }
        
        public virtual void ScenarioInitialize(TechTalk.SpecFlow.ScenarioInfo scenarioInfo)
        {
            testRunner.OnScenarioInitialize(scenarioInfo);
            testRunner.ScenarioContext.ScenarioContainer.RegisterInstanceAs<NUnit.Framework.TestContext>(NUnit.Framework.TestContext.CurrentContext);
        }
        
        public virtual void ScenarioStart()
        {
            testRunner.OnScenarioStart();
        }
        
        public virtual void ScenarioCleanup()
        {
            testRunner.CollectScenarioErrors();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Message Type")]
        public virtual void MessageType()
        {
            string[] tagsOfScenario = ((string[])(null));
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Message Type", null, ((string[])(null)));
#line 8
this.ScenarioInitialize(scenarioInfo);
#line hidden
            bool isScenarioIgnored = default(bool);
            bool isFeatureIgnored = default(bool);
            if ((tagsOfScenario != null))
            {
                isScenarioIgnored = tagsOfScenario.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((this._featureTags != null))
            {
                isFeatureIgnored = this._featureTags.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((isScenarioIgnored || isFeatureIgnored))
            {
                testRunner.SkipScenario();
            }
            else
            {
                this.ScenarioStart();
#line 9
    testRunner.When("I parse \'K>eq`d@000000000\' with padding 0 as a Long Range Ais Broadcast", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 10
    testRunner.Then("NmeaAisLongRangeAisBroadcastParser.Type is 27", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Repeat Indicator")]
        [NUnit.Framework.TestCaseAttribute("K000000000000000", "0", "0", null)]
        [NUnit.Framework.TestCaseAttribute("K@00000000000000", "0", "1", null)]
        [NUnit.Framework.TestCaseAttribute("KP00000000000000", "0", "2", null)]
        [NUnit.Framework.TestCaseAttribute("Kh00000000000000", "0", "3", null)]
        public virtual void RepeatIndicator(string payload, string padding, string repeatCount, string[] exampleTags)
        {
            string[] tagsOfScenario = exampleTags;
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Repeat Indicator", null, exampleTags);
#line 12
this.ScenarioInitialize(scenarioInfo);
#line hidden
            bool isScenarioIgnored = default(bool);
            bool isFeatureIgnored = default(bool);
            if ((tagsOfScenario != null))
            {
                isScenarioIgnored = tagsOfScenario.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((this._featureTags != null))
            {
                isFeatureIgnored = this._featureTags.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((isScenarioIgnored || isFeatureIgnored))
            {
                testRunner.SkipScenario();
            }
            else
            {
                this.ScenarioStart();
#line 13
    testRunner.When(string.Format("I parse \'{0}\' with padding {1} as a Long Range Ais Broadcast", payload, padding), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 14
    testRunner.Then(string.Format("NmeaAisLongRangeAisBroadcastParser.RepeatIndicator is {0}", repeatCount), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("MMSI")]
        [NUnit.Framework.TestCaseAttribute("K000000000000000", "0", "0", null)]
        [NUnit.Framework.TestCaseAttribute("K00000@000000000", "0", "1", null)]
        [NUnit.Framework.TestCaseAttribute("K00000P000000000", "0", "2", null)]
        public virtual void MMSI(string payload, string padding, string mmsi, string[] exampleTags)
        {
            string[] tagsOfScenario = exampleTags;
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("MMSI", null, exampleTags);
#line 23
this.ScenarioInitialize(scenarioInfo);
#line hidden
            bool isScenarioIgnored = default(bool);
            bool isFeatureIgnored = default(bool);
            if ((tagsOfScenario != null))
            {
                isScenarioIgnored = tagsOfScenario.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((this._featureTags != null))
            {
                isFeatureIgnored = this._featureTags.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((isScenarioIgnored || isFeatureIgnored))
            {
                testRunner.SkipScenario();
            }
            else
            {
                this.ScenarioStart();
#line 24
    testRunner.When(string.Format("I parse \'{0}\' with padding {1} as a Long Range Ais Broadcast", payload, padding), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 25
    testRunner.Then(string.Format("NmeaAisLongRangeAisBroadcastParser.Mmsi is {0}", mmsi), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Position Accuracy")]
        [NUnit.Framework.TestCaseAttribute("K000000000000000", "0", "false", null)]
        [NUnit.Framework.TestCaseAttribute("K000008000000000", "0", "true", null)]
        public virtual void PositionAccuracy(string payload, string padding, string positionAccuracy, string[] exampleTags)
        {
            string[] tagsOfScenario = exampleTags;
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Position Accuracy", null, exampleTags);
#line 33
this.ScenarioInitialize(scenarioInfo);
#line hidden
            bool isScenarioIgnored = default(bool);
            bool isFeatureIgnored = default(bool);
            if ((tagsOfScenario != null))
            {
                isScenarioIgnored = tagsOfScenario.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((this._featureTags != null))
            {
                isFeatureIgnored = this._featureTags.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((isScenarioIgnored || isFeatureIgnored))
            {
                testRunner.SkipScenario();
            }
            else
            {
                this.ScenarioStart();
#line 34
    testRunner.When(string.Format("I parse \'{0}\' with padding {1} as a Long Range Ais Broadcast", payload, padding), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 35
    testRunner.Then(string.Format("NmeaAisLongRangeAisBroadcastParser.PositionAccuracy is {0}", positionAccuracy), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Raim Flag")]
        [NUnit.Framework.TestCaseAttribute("K000000000000000", "0", "false", null)]
        [NUnit.Framework.TestCaseAttribute("K000004000000000", "0", "true", null)]
        public virtual void RaimFlag(string payload, string padding, string flag, string[] exampleTags)
        {
            string[] tagsOfScenario = exampleTags;
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Raim Flag", null, exampleTags);
#line 42
this.ScenarioInitialize(scenarioInfo);
#line hidden
            bool isScenarioIgnored = default(bool);
            bool isFeatureIgnored = default(bool);
            if ((tagsOfScenario != null))
            {
                isScenarioIgnored = tagsOfScenario.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((this._featureTags != null))
            {
                isFeatureIgnored = this._featureTags.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((isScenarioIgnored || isFeatureIgnored))
            {
                testRunner.SkipScenario();
            }
            else
            {
                this.ScenarioStart();
#line 43
    testRunner.When(string.Format("I parse \'{0}\' with padding {1} as a Long Range Ais Broadcast", payload, padding), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 44
    testRunner.Then(string.Format("NmeaAisLongRangeAisBroadcastParser.RaimFlag is {0}", flag), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Navigation Status")]
        [NUnit.Framework.TestCaseAttribute("K000000000000000", "0", "UnderwayUsingEngine", null)]
        [NUnit.Framework.TestCaseAttribute("K000000@00000000", "0", "AtAnchor", null)]
        [NUnit.Framework.TestCaseAttribute("K000000P00000000", "0", "NotUnderCommand", null)]
        [NUnit.Framework.TestCaseAttribute("K000000h00000000", "0", "RestrictedManoeuverability", null)]
        [NUnit.Framework.TestCaseAttribute("K000001000000000", "0", "ConstrainedByHerDraught", null)]
        [NUnit.Framework.TestCaseAttribute("K000001@00000000", "0", "Moored", null)]
        [NUnit.Framework.TestCaseAttribute("K000002000000000", "0", "UnderWaySailing", null)]
        [NUnit.Framework.TestCaseAttribute("K000003P00000000", "0", "AisSartIsActive", null)]
        public virtual void NavigationStatus(string payload, string padding, string navigationStatus, string[] exampleTags)
        {
            string[] tagsOfScenario = exampleTags;
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Navigation Status", null, exampleTags);
#line 51
this.ScenarioInitialize(scenarioInfo);
#line hidden
            bool isScenarioIgnored = default(bool);
            bool isFeatureIgnored = default(bool);
            if ((tagsOfScenario != null))
            {
                isScenarioIgnored = tagsOfScenario.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((this._featureTags != null))
            {
                isFeatureIgnored = this._featureTags.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((isScenarioIgnored || isFeatureIgnored))
            {
                testRunner.SkipScenario();
            }
            else
            {
                this.ScenarioStart();
#line 52
    testRunner.When(string.Format("I parse \'{0}\' with padding {1} as a Long Range Ais Broadcast", payload, padding), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 53
    testRunner.Then(string.Format("NmeaAisLongRangeAisBroadcastParser.NavigationStatus is {0}", navigationStatus), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Longitude and Latitute")]
        [NUnit.Framework.TestCaseAttribute("K000000000000000", "0", "0", "0", null)]
        [NUnit.Framework.TestCaseAttribute("K000000000@00000", "0", "1", "0", null)]
        [NUnit.Framework.TestCaseAttribute("K000000000000P00", "0", "0", "1", null)]
        [NUnit.Framework.TestCaseAttribute("K000000?wwh00000", "0", "-1", "0", null)]
        [NUnit.Framework.TestCaseAttribute("K000000000?wwP00", "0", "0", "-1", null)]
        [NUnit.Framework.TestCaseAttribute("K000000000000000", "0", "0", "0", null)]
        public virtual void LongitudeAndLatitute(string payload, string padding, string longitude, string latitude, string[] exampleTags)
        {
            string[] tagsOfScenario = exampleTags;
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Longitude and Latitute", null, exampleTags);
#line 66
this.ScenarioInitialize(scenarioInfo);
#line hidden
            bool isScenarioIgnored = default(bool);
            bool isFeatureIgnored = default(bool);
            if ((tagsOfScenario != null))
            {
                isScenarioIgnored = tagsOfScenario.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((this._featureTags != null))
            {
                isFeatureIgnored = this._featureTags.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((isScenarioIgnored || isFeatureIgnored))
            {
                testRunner.SkipScenario();
            }
            else
            {
                this.ScenarioStart();
#line 67
    testRunner.When(string.Format("I parse \'{0}\' with padding {1} as a Long Range Ais Broadcast", payload, padding), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 68
    testRunner.Then(string.Format("NmeaAisLongRangeAisBroadcastParser.Longitude10thMins is {0}", longitude), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
#line 69
    testRunner.Then(string.Format("NmeaAisLongRangeAisBroadcastParser.Latitude10thMins is {0}", latitude), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Speed Over Ground")]
        [NUnit.Framework.TestCaseAttribute("K000000000000000", "0", "0", null)]
        [NUnit.Framework.TestCaseAttribute("K0000000000000P0", "0", "1", null)]
        [NUnit.Framework.TestCaseAttribute("K000000000000OP0", "0", "63", null)]
        public virtual void SpeedOverGround(string payload, string padding, string speedOverGround, string[] exampleTags)
        {
            string[] tagsOfScenario = exampleTags;
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Speed Over Ground", null, exampleTags);
#line 80
this.ScenarioInitialize(scenarioInfo);
#line hidden
            bool isScenarioIgnored = default(bool);
            bool isFeatureIgnored = default(bool);
            if ((tagsOfScenario != null))
            {
                isScenarioIgnored = tagsOfScenario.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((this._featureTags != null))
            {
                isFeatureIgnored = this._featureTags.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((isScenarioIgnored || isFeatureIgnored))
            {
                testRunner.SkipScenario();
            }
            else
            {
                this.ScenarioStart();
#line 81
    testRunner.When(string.Format("I parse \'{0}\' with padding {1} as a Long Range Ais Broadcast", payload, padding), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 82
    testRunner.Then(string.Format("NmeaAisLongRangeAisBroadcastParser.SpeedOverGroundTenths is {0}", speedOverGround), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Course Over Ground")]
        [NUnit.Framework.TestCaseAttribute("K000000000000000", "0", "0", null)]
        [NUnit.Framework.TestCaseAttribute("K000000000000004", "0", "1", null)]
        [NUnit.Framework.TestCaseAttribute("K000000000000010", "0", "16", null)]
        [NUnit.Framework.TestCaseAttribute("K0000000000000FL", "0", "359", null)]
        [NUnit.Framework.TestCaseAttribute("K0000000000000Ot", "0", "511", null)]
        public virtual void CourseOverGround(string payload, string padding, string courseOverGround, string[] exampleTags)
        {
            string[] tagsOfScenario = exampleTags;
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Course Over Ground", null, exampleTags);
#line 90
this.ScenarioInitialize(scenarioInfo);
#line hidden
            bool isScenarioIgnored = default(bool);
            bool isFeatureIgnored = default(bool);
            if ((tagsOfScenario != null))
            {
                isScenarioIgnored = tagsOfScenario.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((this._featureTags != null))
            {
                isFeatureIgnored = this._featureTags.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((isScenarioIgnored || isFeatureIgnored))
            {
                testRunner.SkipScenario();
            }
            else
            {
                this.ScenarioStart();
#line 91
    testRunner.When(string.Format("I parse \'{0}\' with padding {1} as a Long Range Ais Broadcast", payload, padding), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 92
    testRunner.Then(string.Format("NmeaAisLongRangeAisBroadcastParser.CourseOverGroundDegrees is {0}", courseOverGround), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Gnss Position Status")]
        [NUnit.Framework.TestCaseAttribute("K000000000000000", "0", "false", null)]
        [NUnit.Framework.TestCaseAttribute("K000000000000002", "0", "true", null)]
        public virtual void GnssPositionStatus(string payload, string padding, string gnssStatus, string[] exampleTags)
        {
            string[] tagsOfScenario = exampleTags;
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Gnss Position Status", null, exampleTags);
#line 102
this.ScenarioInitialize(scenarioInfo);
#line hidden
            bool isScenarioIgnored = default(bool);
            bool isFeatureIgnored = default(bool);
            if ((tagsOfScenario != null))
            {
                isScenarioIgnored = tagsOfScenario.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((this._featureTags != null))
            {
                isFeatureIgnored = this._featureTags.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((isScenarioIgnored || isFeatureIgnored))
            {
                testRunner.SkipScenario();
            }
            else
            {
                this.ScenarioStart();
#line 103
    testRunner.When(string.Format("I parse \'{0}\' with padding {1} as a Long Range Ais Broadcast", payload, padding), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 104
    testRunner.Then(string.Format("NmeaAisLongRangeAisBroadcastParser.NotGnssPosition is {0}", gnssStatus), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Spare")]
        [NUnit.Framework.TestCaseAttribute("K000000000000000", "0", "false", null)]
        [NUnit.Framework.TestCaseAttribute("K000000000000001", "0", "true", null)]
        public virtual void Spare(string payload, string padding, string flag, string[] exampleTags)
        {
            string[] tagsOfScenario = exampleTags;
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Spare", null, exampleTags);
#line 111
this.ScenarioInitialize(scenarioInfo);
#line hidden
            bool isScenarioIgnored = default(bool);
            bool isFeatureIgnored = default(bool);
            if ((tagsOfScenario != null))
            {
                isScenarioIgnored = tagsOfScenario.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((this._featureTags != null))
            {
                isFeatureIgnored = this._featureTags.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((isScenarioIgnored || isFeatureIgnored))
            {
                testRunner.SkipScenario();
            }
            else
            {
                this.ScenarioStart();
#line 112
    testRunner.When(string.Format("I parse \'{0}\' with padding {1} as a Long Range Ais Broadcast", payload, padding), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 113
    testRunner.Then(string.Format("NmeaAisLongRangeAisBroadcastParser.Spare95 is {0}", flag), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            }
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
