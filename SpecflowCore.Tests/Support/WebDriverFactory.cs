using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace SpecflowCore.Tests.Support
{
    /// <summary>
    /// Factory for web drivers
    /// This ensures that all web driver instances are configured consistently
    /// </summary>
    public static class WebDriverFactory
    {
        public static IWebDriver CreateDriver()
        {
            var options = new ChromeOptions();
            options.AddArgument("--start-maximized");
            options.AddArgument("--window-size=1920,1080");
            
            return new ChromeDriver(options);
        }
    }
}