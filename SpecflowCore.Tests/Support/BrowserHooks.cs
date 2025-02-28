using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using BoDi;
using TechTalk.SpecFlow;
using System;
using System.IO;

namespace SpecflowCore.Tests.Support
{
    [Binding]
    public class BrowserHooks
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;
        private IWebDriver? _driver;

        public BrowserHooks(IObjectContainer objectContainer, ScenarioContext scenarioContext)
        {
            _objectContainer = objectContainer;
            _scenarioContext = scenarioContext;
        }

        [BeforeScenario(Order = 0)]
        public void CreateWebDriver()
        {
            try
            {
                var options = new ChromeOptions();
                options.AddArgument("--headless");
                options.AddArgument("--start-maximized");
                options.AddArgument("--window-size=1920,1080");
                
                _driver = new ChromeDriver(options);
                _objectContainer.RegisterInstanceAs(_driver);
                
                Console.WriteLine("WebDriver created successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to create WebDriver: {ex.Message}");
                throw;
            }
        }

        [AfterScenario(Order = 0)] // Run before Hooks.AfterScenario
        public void CaptureScreenshot()
        {
            try
            {
                if (_driver == null)
                {
                    Console.WriteLine("Cannot take screenshot - WebDriver is null");
                    return;
                }

                // Always take a screenshot, whether the test passed or failed
                var timestamp = DateTime.Now.ToString("HHmmss");
                var screenshotName = $"{_scenarioContext.ScenarioInfo.Title.Replace(" ", "_")}_{timestamp}.png";
                var screenshotPath = Path.Combine(TestRunContext.ScreenshotsPath, screenshotName);

                // Ensure we're at the top of the page for consistent screenshots
                ((IJavaScriptExecutor)_driver).ExecuteScript("window.scrollTo(0, 0)");
                
                Console.WriteLine($"Taking screenshot: {screenshotPath}");
                var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
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
        public void CloseWebDriver()
        {
            try
            {
                if (_driver != null)
                {
                    _driver.Quit();
                    _driver.Dispose();
                    _driver = null;
                    Console.WriteLine("WebDriver closed and disposed successfully");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to close WebDriver: {ex.Message}");
                // Ensure driver is nulled even if cleanup fails
                _driver = null;
            }
        }
    }
}
