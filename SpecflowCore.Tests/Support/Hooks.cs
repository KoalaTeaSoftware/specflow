using System;
using System.IO;
using OpenQA.Selenium;
using TechTalk.SpecFlow;

namespace SpecflowCore.Tests.Support
{
    [Binding]
    public class Hooks
    {
        private static readonly string TestResultsPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", "..", "TestResults");

        [AfterScenario]
        public void AfterScenario(ScenarioContext scenarioContext)
        {
            if (scenarioContext.TestError != null)
            {
                TakeScreenshot(scenarioContext);
            }
            BrowserContext.Instance.CleanupContext();
        }

        private void TakeScreenshot(ScenarioContext scenarioContext)
        {
            try
            {
                var driver = BrowserContext.Instance.Driver;
                if (driver != null)
                {
                    var screenshot = ((ITakesScreenshot)driver).GetScreenshot();
                    
                    // Create Screenshots directory in the TestResults folder
                    string screenshotDirectory = Path.Combine(TestResultsPath, "Screenshots");
                    Directory.CreateDirectory(screenshotDirectory);

                    // Create a unique filename using timestamp and scenario title
                    string fileName = $"{DateTime.Now:yyyyMMdd_HHmmss}_{scenarioContext.ScenarioInfo.Title.Replace(" ", "_")}.png";
                    string filePath = Path.Combine(screenshotDirectory, fileName);

                    // Save the screenshot
                    File.WriteAllBytes(filePath, screenshot.AsByteArray);
                    Console.WriteLine($"Screenshot saved: {filePath}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to take screenshot: {ex.Message}");
            }
        }
    }
}