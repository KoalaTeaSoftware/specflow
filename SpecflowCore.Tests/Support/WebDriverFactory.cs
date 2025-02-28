using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace SpecflowCore.Tests.Support
{
    /// <summary>
    /// Factory for web drivers
    /// This ensures that all web driver instances are configured consistently
    /// </summary>
    public class WebDriverFactory
    {
        public static IWebDriver CreateDriver()
        {
            var options = new ChromeOptions();
            options.AddArgument("--start-maximized");
            options.AddArgument("--no-sandbox");
            options.AddArgument("--disable-dev-shm-usage");
            
            // Enable taking proper screenshots
            options.AddArgument("--disable-gpu");
            options.AddArgument("--force-device-scale-factor=1");
            
            return new ChromeDriver(options);
        }
    }
}