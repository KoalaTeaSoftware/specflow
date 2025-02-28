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
        /// Clicks a link with specific text, optionally within a search context
        /// </summary>
        /// <returns>True if click was successful, false otherwise</returns>
        public static bool ClickLinkWithText(this IWebDriver driver, string linkText, By? searchContext = null)
        {
            searchContext ??= BasePage.Elements.DefaultContext;
            var element = driver.FindElement(By.LinkText(linkText), searchContext);
            if (element != null)
            {
                element.Click();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Clicks a link containing specific text, optionally within a search context
        /// </summary>
        /// <returns>True if click was successful, false otherwise</returns>
        public static bool ClickLinkContainingText(this IWebDriver driver, string partialLinkText, By? searchContext = null)
        {
            searchContext ??= BasePage.Elements.DefaultContext;
            var element = driver.FindElement(By.PartialLinkText(partialLinkText), searchContext);
            if (element != null)
            {
                element.Click();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Gets the text of an element if it exists
        /// </summary>
        /// <returns>The element text or null if element not found</returns>
        public static string? GetText(this IWebDriver driver, By locator, By? searchContext = null)
        {
            searchContext ??= BasePage.Elements.DefaultContext;
            var element = driver.FindElement(locator, searchContext);
            return element?.Text;
        }
    }
}
