using System;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Infrastructure;

namespace SpecflowCore.Tests.Support
{
    [Binding]
    public class Hooks
    {
        private readonly ISpecFlowOutputHelper _outputHelper;
        private readonly ScenarioContext _scenarioContext;
        private readonly FeatureContext _featureContext;
        private static readonly HtmlReportListener ReportListener = new HtmlReportListener();
        private DateTime _scenarioStartTime;

        public Hooks(ISpecFlowOutputHelper outputHelper, ScenarioContext scenarioContext, FeatureContext featureContext)
        {
            _outputHelper = outputHelper;
            _scenarioContext = scenarioContext;
            _featureContext = featureContext;
        }

        [BeforeScenario(Order = 1)]
        public void BeforeScenario()
        {
            // Set the current ScenarioContext for TestRunContext
            TestRunContext.CurrentScenarioContext = _scenarioContext;

            _scenarioStartTime = DateTime.Now;
            var testName = $"{_featureContext.FeatureInfo.Title} - {_scenarioContext.ScenarioInfo.Title}";
            ReportListener.OnTestStart(testName);
            _outputHelper.WriteLine($"Starting test: {testName}");
        }

        [AfterScenario(Order = 1)]
        public void AfterScenario()
        {
            var testName = $"{_featureContext.FeatureInfo.Title} - {_scenarioContext.ScenarioInfo.Title}";
            var duration = DateTime.Now - _scenarioStartTime;

            if (_scenarioContext.TestError != null)
            {
                ReportListener.OnTestFail(
                    testName,
                    duration,
                    _scenarioContext.TestError.Message,
                    _scenarioContext.TestError.StackTrace ?? string.Empty,
                    _scenarioContext.ContainsKey("LastScreenshotPath") 
                        ? _scenarioContext["LastScreenshotPath"]?.ToString() ?? string.Empty 
                        : string.Empty
                );
                _outputHelper.WriteLine($"Test failed: {testName}");
                _outputHelper.WriteLine($"Error: {_scenarioContext.TestError.Message}");
            }
            else
            {
                ReportListener.OnTestPass(testName, duration);
                _outputHelper.WriteLine($"Test passed: {testName}");
            }

            // Clear the current ScenarioContext
            TestRunContext.CurrentScenarioContext = null;
        }

        [AfterTestRun]
        public static void AfterTestRun()
        {
            ReportListener.SaveReport();
        }
    }
}