using OpenQA.Selenium;
using NUnit.Framework;

namespace SpecflowCore.Tests.POM
{
    /// <summary>
    /// Provides basic element interaction methods that can be used across all pages.
    /// These are the fundamental actions that don't involve waiting or complex logic.
    /// </summary>
    public static class BaseElementActions
    {
        /// <summary>
        /// Finds an element within a specified search context
        /// </summary>
        /// <param name="driver">The WebDriver instance</param>
        /// <param name="locator">The locator for the target element</param>
        /// <param name="searchContext">Optional context to search within, defaults to body</param>
        /// <returns>The found element or null if not found</returns>
        public static IWebElement? FindElement(this IWebDriver driver, By locator, By? searchContext = null)
        {
            try
            {
                if (searchContext == null)
                {
                    return driver.FindElement(locator);
                }

                var context = driver.FindElement(searchContext);
                return context?.FindElement(locator);
            }
            catch (NoSuchElementException)
            {
                TestContext.WriteLine($"Failed to find element '{locator}' within parent context '{searchContext ?? BasePage.Elements.DefaultContext}'.");
                return null;
            }
        }

        /// <summary>
        /// Clicks an element if it exists
        /// </summary>
        public static void Click(this IWebDriver driver, By locator, By? searchContext = null)
        {
            driver.FindElement(locator, searchContext)?.Click();
        }

        /// <summary>
        /// Types text into an element if it exists
        /// </summary>
        public static void Type(this IWebDriver driver, By locator, string text, By? searchContext = null)
        {
            driver.FindElement(locator, searchContext)?.SendKeys(text);
        }

        /// <summary>
        /// Gets the text of an element if it exists
        /// </summary>
        public static string GetText(this IWebDriver driver, By locator, By? searchContext = null)
        {
            return driver.FindElement(locator, searchContext)?.Text;
        }
    }
}
