using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using TechTalk.SpecFlow;

namespace SpecflowCore.Tests.Support
{
    /// <summary>
    /// Factory for web drivers
    ///This ensures that all web driver instances are all look the same
    /// </summary>
    public class WebDriverFactory
    {
        public static IWebDriver CreateDriver()
        {
           var options = new ChromeOptions();
            // Add any Chrome options here if needed
            // options.AddArgument("--headless");
            
            return new ChromeDriver(options);
        }
    }
}