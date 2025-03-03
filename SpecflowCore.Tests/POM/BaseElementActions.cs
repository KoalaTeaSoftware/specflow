using OpenQA.Selenium;
using System.Collections.Generic;
using System.Linq;

namespace SpecflowCore.Tests.POM
{
    public static class BaseElementActions
    {
        /// <summary>
        /// Finds an element within an optional search context
        /// </summary>
        public static IWebElement FindElement(
            this IWebDriver driver,
            By locator,
            By? searchContext = null)
        {
            if (searchContext == null)
            {
                // Uses driver.FindElement directly because this is our base implementation
                return driver.FindElement(locator);
            }

            // Uses context.FindElement directly because this is our base implementation
            var context = driver.FindElement(searchContext);
            return context.FindElement(locator);
        }

        /// <summary>
        /// Finds all elements matching a locator within an optional search context
        /// </summary>
        public static IReadOnlyCollection<IWebElement> FindElements(
            this IWebDriver driver,
            By locator,
            By? searchContext = null)
        {
            if (searchContext == null)
            {
                // Uses driver.FindElements directly because this is our base implementation
                return driver.FindElements(locator);
            }

            // Uses context.FindElements directly because this is our base implementation
            var context = driver.FindElement(searchContext);
            return context.FindElements(locator);
        }

        /// <summary>
        /// Gets text of an element if it exists, null otherwise
        /// </summary>
        public static string? GetText(
            this IWebDriver driver,
            By locator,
            By? searchContext = null)
        {
            // Uses our extension method for consistency
            var element = driver.FindElement(locator, searchContext);
            return element?.Text;
        }

        /// <summary>
        /// Clicks a link with specific text within an optional search context
        /// </summary>
        public static bool ClickLinkWithText(
            this IWebDriver driver,
            string linkText,
            By? searchContext = null)
        {
            // Uses our extension method for consistency
            var links = driver.FindElements(By.TagName("a"), searchContext);
            var link = links.FirstOrDefault(l => l.Text.Contains(linkText));

            if (link == null)
            {
                return false;
            }

            link.Click();
            return true;
        }

        /// <summary>
        /// Clicks a link containing specific text within an optional search context
        /// </summary>
        public static bool ClickLinkContainingText(
            this IWebDriver driver,
            string partialText,
            By? searchContext = null)
        {
            // Uses our extension method for consistency
            var links = driver.FindElements(By.TagName("a"), searchContext);
            var link = links.FirstOrDefault(l => l.Text.Contains(partialText));

            if (link == null)
            {
                return false;
            }

            link.Click();
            return true;
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
    }
}
