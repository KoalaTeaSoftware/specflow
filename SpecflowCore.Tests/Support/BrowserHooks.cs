using TechTalk.SpecFlow;
using System;
using OpenQA.Selenium;
using System.IO;
using SpecflowCore.Tests.Support;

namespace SpecflowCore.Tests.Support
{
    [Binding]
    public class BrowserHooks
    {
        private readonly ScenarioContext _scenarioContext;

        public BrowserHooks(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [AfterScenario(Order = 1)]
        public void AfterScenario()
        {
            // Only reset browser if the scenario requires a fresh browser
            if (_scenarioContext.ScenarioInfo.Tags.Contains("reset_browser"))
            {
                try
                {
                    BrowserContext.Instance.Reset();
                    Console.WriteLine("WebDriver reset successfully");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to reset WebDriver: {ex.Message}");
                }
            }
        }

        [AfterTestRun]
        public static void AfterTestRun()
        {
            try
            {
                BrowserContext.Instance.Dispose();
                Console.WriteLine("WebDriver disposed successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to dispose WebDriver: {ex.Message}");
            }
        }
    }
}
