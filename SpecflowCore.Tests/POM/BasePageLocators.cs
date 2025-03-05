using OpenQA.Selenium;

namespace SpecflowCore.Tests.POM
{
    /// <summary>
    /// Base class containing common element definitions and properties used across all pages.
    /// Centralizes the locator strategies for common elements.
    /// </summary>
    public class BasePageLocators
    {
        private readonly IWebDriver _driver;

        /// <summary>
        /// Initializes a new instance of the <see cref="BasePageLocators"/> class.
        /// </summary>
        /// <param name="driver">The Selenium WebDriver instance.</param>
        public BasePageLocators(IWebDriver driver)
        {
            _driver = driver;
        }

        // Static elements for consistent access
        public static class Elements
        {
            // Primary content container - using ID as required
            public static readonly By MainContent = By.Id("pageContents");

            // Main heading - using CSS selector since h1 is unique per page
            public static readonly By MainHeading = By.CssSelector("#pageContents h1");

            // Default context for searches - using ID as required
            public static readonly By DefaultContext = By.Id("pageContents");
        }            
    }
}