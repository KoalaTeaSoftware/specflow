using OpenQA.Selenium;
using BoDi;
using TechTalk.SpecFlow;
using System;
using System.IO;
using System.Threading;

namespace SpecflowCore.Tests.Support
{
    [Binding]
    public class BrowserHooks
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        public BrowserHooks(IObjectContainer objectContainer, ScenarioContext scenarioContext)
        {
            _objectContainer = objectContainer;
            _scenarioContext = scenarioContext;
        }

        [BeforeScenario(Order = 0)]
        public void SetupWebDriver()
        {
            try
            {
                // Get the driver from BrowserContext and register it with the container
                var driver = BrowserContext.Instance.Driver;
                _objectContainer.RegisterInstanceAs(driver);
                Console.WriteLine("WebDriver registered successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to setup WebDriver: {ex.Message}");
                throw;
            }
        }

        [AfterScenario(Order = 0)] // Run before Hooks.AfterScenario
        public void CaptureScreenshot()
        {
            try
            {
                var driver = BrowserContext.Instance.Driver;

                // Give the page a moment to settle
                Thread.Sleep(500);

                // Always take a screenshot, whether the test passed or failed
                var timestamp = DateTime.Now.ToString("HHmmss");
                var screenshotName = $"{_scenarioContext.ScenarioInfo.Title.Replace(" ", "_")}_{timestamp}.png";
                var screenshotPath = Path.Combine(TestRunContext.ScreenshotsPath, screenshotName);

                // Ensure we're at the top of the page for consistent screenshots
                ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollTo(0, 0)");
                
                // Give the page another moment to settle after scrolling
                Thread.Sleep(500);
                
                Console.WriteLine($"Taking screenshot: {screenshotPath}");
                var screenshot = ((ITakesScreenshot)driver).GetScreenshot();
                screenshot.SaveAsFile(screenshotPath);
                
                // Store the screenshot path in ScenarioContext for the report
                _scenarioContext["LastScreenshotPath"] = screenshotPath;
                
                Console.WriteLine($"Screenshot saved successfully to: {screenshotPath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to take screenshot: {ex.Message}");
                // Don't throw as we want to continue with cleanup
            }
        }

        [AfterScenario(Order = 99)] // Run last
        public void CleanupWebDriver()
        {
            try
            {
                BrowserContext.Instance.CleanupContext();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to cleanup WebDriver: {ex.Message}");
            }
        }
    }
}
