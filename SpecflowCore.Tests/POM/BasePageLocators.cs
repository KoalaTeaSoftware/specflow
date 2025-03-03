using OpenQA.Selenium;

namespace SpecflowCore.Tests.POM
{
    /// <summary>
    /// Base class containing common element definitions and properties used across all pages.
    /// Centralizes the locator strategies for common elements.
    /// </summary>
    public static class BasePageLocators
    {
        /// <summary>
        /// Common element locators used across pages
        /// </summary>
        public static class Elements
        {
            /// <summary>
            /// Default search context for element location operations
            /// </summary>
            public static readonly By DefaultContext = By.TagName("body");

            /// <summary>
            /// Main heading of the page. Using h1 by default, but can be changed if site uses different heading hierarchy
            /// </summary>
            public static readonly By MainHeading = By.CssSelector("h1");

            /// <summary>
            /// Main navigation menu
            /// </summary>
            public static readonly By MainNav = By.CssSelector("nav.main-nav");

            /// <summary>
            /// Footer section
            /// </summary>
            public static readonly By Footer = By.TagName("footer");
        }
    }
}