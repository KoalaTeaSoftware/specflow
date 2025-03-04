using OpenQA.Selenium;
using System.Collections.Generic;
using System.Linq;
using SpecflowCore.Tests.Support;
using NUnit.Framework;

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
            IWebElement? searchContext = null)
        {
            if (searchContext == null)
            {
                return driver.FindElement(locator);
            }

            return searchContext.FindElement(locator);
        }

        /// <summary>
        /// Finds all elements matching a locator within an optional search context
        /// </summary>
        public static IReadOnlyCollection<IWebElement> FindElements(
            this IWebDriver driver,
            By locator,
            IWebElement? searchContext = null)
        {
            if (searchContext == null)
            {
                return driver.FindElements(locator);
            }

            return searchContext.FindElements(locator);
        }

        /// <summary>
        /// Gets text of an element if it exists, null otherwise
        /// </summary>
        public static string? GetText(
            this IWebDriver driver,
            By locator,
            IWebElement? searchContext = null)
        {
            var element = driver.FindElement(locator, searchContext);
            return element?.Text;
        }

        /// <summary>
        /// Clicks a link with specific text within an optional search context
        /// </summary>
        public static bool ClickLinkWithText(
            this IWebDriver driver,
            string linkText,
            IWebElement? searchContext = null)
        {
            var container = searchContext ?? driver.FindElement(By.TagName("body"));
            var links = container.FindElements(By.TagName("a"));
            var link = links.FirstOrDefault(l => l.Text.Contains(linkText));

            if (link == null)
            {
                var path = BrowserContext.Instance.CaptureFailureScreenshot($"link_with_text_not_found_{linkText}");
                TestContext.WriteLine($"Could not find link with text '{linkText}'. Screenshot: {path}");
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
            IWebElement? searchContext = null)
        {
            var container = searchContext ?? driver.FindElement(By.TagName("body"));
            var links = container.FindElements(By.TagName("a"));
            var link = links.FirstOrDefault(l => l.Text.Contains(partialText));

            if (link == null)
            {
                var path = BrowserContext.Instance.CaptureFailureScreenshot($"link_containing_text_not_found_{partialText}");
                TestContext.WriteLine($"Could not find link containing text '{partialText}'. Screenshot: {path}");
                return false;
            }

            link.Click();
            return true;
        }

        /// <summary>
        /// Clicks an element if it exists
        /// </summary>
        public static void Click(
            this IWebDriver driver,
            By locator,
            IWebElement? searchContext = null)
        {
            try
            {
                driver.FindElement(locator, searchContext)?.Click();
            }
            catch (ElementClickInterceptedException ex)
            {
                var path = BrowserContext.Instance.CaptureFailureScreenshot($"click_intercepted_{locator.ToString().Replace('/', '_')}");
                throw new ElementClickInterceptedException($"Element click was intercepted. Screenshot: {path}", ex);
            }
            catch (ElementNotInteractableException ex)
            {
                var path = BrowserContext.Instance.CaptureFailureScreenshot($"element_not_interactable_{locator.ToString().Replace('/', '_')}");
                throw new ElementNotInteractableException($"Element is not interactable. Screenshot: {path}", ex);
            }
        }

        /// <summary>
        /// Types text into an element if it exists
        /// </summary>
        public static void Type(
            this IWebDriver driver,
            By locator,
            string text,
            IWebElement? searchContext = null)
        {
            try
            {
                driver.FindElement(locator, searchContext)?.SendKeys(text);
            }
            catch (ElementNotInteractableException ex)
            {
                var path = BrowserContext.Instance.CaptureFailureScreenshot($"cannot_type_{locator.ToString().Replace('/', '_')}");
                throw new ElementNotInteractableException($"Cannot type into element. Screenshot: {path}", ex);
            }
        }
    }
}
