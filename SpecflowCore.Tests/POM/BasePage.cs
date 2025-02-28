using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SpecflowCore.Tests.Support;
using NUnit.Framework;

namespace SpecflowCore.Tests.POM
{
    /// <summary>
    /// Base class containing common element definitions and properties used across all pages.
    /// Centralizes the locator strategies for common elements.
    /// </summary>
    public abstract class BasePage
    {
        /// <summary>
        /// Gets the WebDriver instance from the BrowserContext
        /// </summary>
        public IWebDriver Driver => BrowserContext.Instance.Driver;

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

        private readonly By _defaultSearchContext = Elements.DefaultContext;

        public IWebElement WaitForElement(By childLocator, int timeoutSeconds = 10)
        {
            return WaitForElement(childLocator, _defaultSearchContext, timeoutSeconds);
        }

        public IWebElement WaitForElement(By childLocator, By searchContext, int timeoutSeconds = 10)
        {
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(timeoutSeconds));
            return wait.Until(d => {
                var context = d.FindElement(searchContext);
                return context.FindElement(childLocator);
            });
        }

        public IWebElement WaitForElementToHaveText(By locator, string expectedText, int timeoutSeconds = 10)
        {
            return WaitForElementToHaveText(locator, _defaultSearchContext, expectedText, timeoutSeconds);
        }

        public IWebElement WaitForElementToHaveText(By locator, By searchContext, string expectedText, int timeoutSeconds = 10)
        {
            try
            {
                var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(timeoutSeconds));
                return wait.Until(d => {
                    var element = FindElement(locator, searchContext);
                    return (element != null && element.Text.Contains(expectedText)) ? element : null;
                });
            }
            catch (WebDriverTimeoutException)
            {
                NUnit.Framework.TestContext.WriteLine($"Timeout waiting for element '{locator}' to have text '{expectedText}' within parent context '{searchContext}'. Element was not found or text did not match.");
                return null;
            }
        }

        public IWebElement WaitForElementWithin(By childLocator, By parentContext, int timeoutSeconds = 8)
        {
            try
            {
                var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(timeoutSeconds));
                return wait.Until(d => FindElement(childLocator, parentContext));
            }
            catch (WebDriverTimeoutException)
            {
                NUnit.Framework.TestContext.WriteLine($"Timeout waiting for element '{childLocator}' within parent context '{parentContext}'. Element was not found.");
                return null;
            }
        }

        public IWebElement FindElement(By locator)
        {
            return FindElement(locator, _defaultSearchContext);
        }

        public IWebElement FindElement(By locator, By searchContext)
        {
            try
            {
                var context = Driver.FindElement(searchContext);
                return context.FindElement(locator);
            }
            catch (NoSuchElementException)
            {
                NUnit.Framework.TestContext.WriteLine($"Failed to find element '{locator}' within parent context '{searchContext}'.");
                return null;
            }
        }

        public void Click(By locator)
        {
            Click(locator, _defaultSearchContext);
        }

        public void Click(By locator, By searchContext)
        {
            FindElement(locator, searchContext)?.Click();
        }

        public void Type(By locator, string text)
        {
            Type(locator, text, _defaultSearchContext);
        }

        public void Type(By locator, string text, By searchContext)
        {
            FindElement(locator, searchContext)?.SendKeys(text);
        }
    }
}