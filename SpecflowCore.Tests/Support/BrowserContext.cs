using OpenQA.Selenium;
using SpecflowCore.Tests.POM;
using System;
using System.IO;

namespace SpecflowCore.Tests.Support
{
    /// <summary>
    /// Singleton class to manage browser context
    /// </summary>
    public class BrowserContext : IDisposable
    {
        private static BrowserContext _instance;
        private static readonly object Lock = new object();
        private bool _disposed;

        private BrowserContext()
        {
            Driver = WebDriverFactory.CreateDriver();
        }

        public IWebDriver Driver { get; private set; }

        public static BrowserContext Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (Lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new BrowserContext();
                        }
                    }
                }
                return _instance;
            }
        }

        public void Reset()
        {
            if (!_disposed)
            {
                Driver?.Quit();
                Driver = WebDriverFactory.CreateDriver();
            }
        }

        public string CaptureFailureScreenshot(string failureContext)
        {
            if (_disposed)
            {
                return string.Empty;
            }

            var screenshot = ((ITakesScreenshot)Driver).GetScreenshot();
            var fileName = $"{failureContext}_{DateTime.Now:yyyyMMdd_HHmmss}.png";
            var path = Path.Combine(TestRunContext.ScreenshotsPath, fileName);
            screenshot.SaveAsFile(path);
            TestRunContext.CurrentScenarioContext["LastScreenshotPath"] = path;
            return path;
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _disposed = true;
                Driver?.Quit();
                Driver?.Dispose();
                Driver = null;
                _instance = null;
            }
        }
    }
}