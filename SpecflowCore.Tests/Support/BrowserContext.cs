using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using SpecflowCore.Tests.POM;
using System;

namespace SpecflowCore.Tests.Support
{
    public class BrowserContext
    {
        private static BrowserContext? _instance;
        private static readonly object _lock = new object();
        
        private IWebDriver? _driver;
        private BasePage? _currentPage;

        private BrowserContext() { }

        public static BrowserContext Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        _instance ??= new BrowserContext();
                    }
                }
                return _instance;
            }
        }

        public IWebDriver Driver
        {
            get
            {
                if (_driver == null)
                {
                    var options = new ChromeOptions();
                    options.AddArgument("--start-maximized");
                    options.AddArgument("--window-size=1920,1080");
                    
                    _driver = new ChromeDriver(options);
                    Console.WriteLine("WebDriver created successfully");
                }
                return _driver;
            }
        }

        public T GetPage<T>() where T : BasePage, new()
        {
            _currentPage = new T();
            return (T)_currentPage;
        }

        public void CleanupContext()
        {
            if (_driver != null)
            {
                try
                {
                    _driver.Close(); // Close the current window first
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Warning: Failed to close browser window: {ex.Message}");
                }

                try
                {
                    _driver.Quit(); // Then quit the browser completely
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Warning: Failed to quit browser: {ex.Message}");
                }

                try
                {
                    _driver.Dispose(); // Finally dispose of resources
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Warning: Failed to dispose driver: {ex.Message}");
                }

                _driver = null;
                Console.WriteLine("WebDriver closed and disposed successfully");
            }
        }
    }
}