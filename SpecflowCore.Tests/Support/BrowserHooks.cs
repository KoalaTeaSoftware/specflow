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
            // Only take screenshot if the test failed
            if (_scenarioContext.TestError == null)
            {
                return;
            }

            try
            {
                var driver = BrowserContext.Instance.Driver;

                // Give the page a moment to settle
                Thread.Sleep(500);

                var timestamp = DateTime.Now.ToString("HHmmss");
                var screenshotName = $"{_scenarioContext.ScenarioInfo.Title.Replace(" ", "_")}_{timestamp}.png";
                var screenshotPath = Path.Combine(TestRunContext.ScreenshotsPath, screenshotName);

                // Ensure we're at the top of the page for consistent screenshots
                ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollTo(0, 0)");
                
                // Give the page another moment to settle after scrolling
                Thread.Sleep(500);
                
                Console.WriteLine($"Taking failure screenshot: {screenshotPath}");
                var screenshot = ((ITakesScreenshot)driver).GetScreenshot();
                screenshot.SaveAsFile(screenshotPath);
                Console.WriteLine($"Screenshot saved successfully to: {screenshotPath}");

                // Store the screenshot path in the scenario context for reporting
                _scenarioContext["LastScreenshotPath"] = screenshotPath;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to capture screenshot: {ex.Message}");
            }
        }

        [AfterScenario(Order = 99)] // Run last
        public void CleanupWebDriver()
        {
            try
            {
                BrowserContext.Instance.CleanupContext();
                Console.WriteLine("WebDriver closed and disposed successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to cleanup WebDriver: {ex.Message}");
            }
        }
    }
}
