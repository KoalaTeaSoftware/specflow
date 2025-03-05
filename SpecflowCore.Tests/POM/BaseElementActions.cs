using OpenQA.Selenium;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using SpecflowCore.Tests.Support;
using NUnit.Framework;

namespace SpecflowCore.Tests.POM
{
    /// <summary>
    /// Provides enhanced base operations for Selenium WebDriver with improved error handling and screenshots.
    /// All methods follow consistent patterns:
    /// - Enhanced error handling with detailed messages
    /// - Automatic screenshot capture on failures
    /// - Support for optional search contexts
    /// - Return values indicating operation success/failure
    /// </summary>
    public static class BaseElementActions
    {
        #region Element Finding

        /// <summary>
        /// Finds an element within an optional search context with enhanced error handling.
        /// Captures screenshots and provides detailed error messages on failure.
        /// </summary>
        /// <param name="driver">The WebDriver instance</param>
        /// <param name="locator">The By locator to find the element</param>
        /// <param name="searchContext">Optional context to search within. If null, searches entire page</param>
        /// <returns>The found IWebElement</returns>
        /// <exception cref="NoSuchElementException">When element cannot be found, includes screenshot path</exception>
        public static IWebElement BaseFindElement(
            this IWebDriver driver,
            By locator,
            IWebElement? searchContext = null)
        {
            try
            {
                return searchContext?.FindElement(locator) ?? driver.FindElement(locator);
            }
            catch (NoSuchElementException)
            {
                var path = BrowserContext.Instance.CaptureFailureScreenshot($"element_not_found_{locator.ToString().Replace('/', '_')}");
                throw new NoSuchElementException($"Element not found with locator: {locator}. Screenshot: {path}");
            }
        }

        /// <summary>
        /// Finds all elements matching a locator within an optional search context with enhanced error handling.
        /// Captures screenshots and provides detailed error messages on failure.
        /// </summary>
        /// <param name="driver">The WebDriver instance</param>
        /// <param name="locator">The By locator to find elements</param>
        /// <param name="searchContext">Optional context to search within. If null, searches entire page</param>
        /// <returns>Collection of found IWebElements</returns>
        /// <exception cref="WebDriverException">When search fails, includes screenshot path</exception>
        public static IReadOnlyCollection<IWebElement> BaseFindElements(
            this IWebDriver driver,
            By locator,
            IWebElement? searchContext = null)
        {
            try
            {
                return searchContext?.FindElements(locator) ?? driver.FindElements(locator);
            }
            catch (WebDriverException ex)
            {
                var path = BrowserContext.Instance.CaptureFailureScreenshot($"find_elements_failed_{locator.ToString().Replace('/', '_')}");
                throw new WebDriverException($"Failed to find elements with locator: {locator}. Screenshot: {path}", ex);
            }
        }

        #endregion

        #region Element Text Operations

        /// <summary>
        /// Gets text from an element if it exists, with enhanced error handling.
        /// Returns null if element cannot be found or accessed.
        /// </summary>
        /// <param name="driver">The WebDriver instance</param>
        /// <param name="locator">The By locator to find the element</param>
        /// <param name="searchContext">Optional context to search within. If null, searches entire page</param>
        /// <returns>The element's text or null if element cannot be found/accessed</returns>
        public static string? BaseGetElementText(
            this IWebDriver driver,
            By locator,
            IWebElement? searchContext = null)
        {
            try
            {
                var element = driver.BaseFindElement(locator, searchContext);
                return element?.Text;
            }
            catch (WebDriverException)
            {
                return null;
            }
        }

        /// <summary>
        /// Types text into an element with enhanced error handling and screenshots.
        /// </summary>
        /// <param name="driver">The WebDriver instance</param>
        /// <param name="locator">The By locator to find the element to type into</param>
        /// <param name="text">The text to type</param>
        /// <param name="searchContext">Optional context to search within. If null, searches entire page</param>
        /// <returns>True if text was successfully typed, false otherwise</returns>
        public static bool BaseTypeText(
            this IWebDriver driver,
            By locator,
            string text,
            IWebElement? searchContext = null)
        {
            try
            {
                driver.BaseFindElement(locator, searchContext)?.SendKeys(text);
                return true;
            }
            catch (ElementNotInteractableException ex)
            {
                var path = BrowserContext.Instance.CaptureFailureScreenshot($"cannot_type_{locator.ToString().Replace('/', '_')}");
                TestContext.WriteLine($"Cannot type into element. Screenshot: {path}");
                return false;
            }
        }

        #endregion

        #region Click Operations

        /// <summary>
        /// Clicks an element with enhanced error handling and screenshots.
        /// </summary>
        /// <param name="driver">The WebDriver instance</param>
        /// <param name="locator">The By locator to find the element to click</param>
        /// <param name="searchContext">Optional context to search within. If null, searches entire page</param>
        /// <returns>True if element was found and clicked successfully, false otherwise</returns>
        public static bool BaseClick(
            this IWebDriver driver,
            By locator,
            IWebElement? searchContext = null)
        {
            try
            {
                driver.BaseFindElement(locator, searchContext)?.Click();
                return true;
            }
            catch (ElementClickInterceptedException ex)
            {
                var path = BrowserContext.Instance.CaptureFailureScreenshot($"click_intercepted_{locator.ToString().Replace('/', '_')}");
                TestContext.WriteLine($"Element click was intercepted. Screenshot: {path}");
                return false;
            }
            catch (ElementNotInteractableException ex)
            {
                var path = BrowserContext.Instance.CaptureFailureScreenshot($"element_not_interactable_{locator.ToString().Replace('/', '_')}");
                TestContext.WriteLine($"Element is not interactable. Screenshot: {path}");
                return false;
            }
        }

        /// <summary>
        /// Executes a JavaScript click on an element, useful for avoiding stale element issues.
        /// </summary>
        /// <param name="driver">The WebDriver instance</param>
        /// <param name="element">The element to click</param>
        /// <returns>True if JavaScript click was successful, false otherwise</returns>
        public static bool BaseJavaScriptClick(
            this IWebDriver driver,
            IWebElement element)
        {
            try
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", element);
                return true;
            }
            catch (WebDriverException ex)
            {
                var path = BrowserContext.Instance.CaptureFailureScreenshot("javascript_click_failed");
                TestContext.WriteLine($"JavaScript click failed. Error: {ex.Message}. Screenshot: {path}");
                return false;
            }
        }

        /// <summary>
        /// Clicks a link matching the specified regular expression pattern.
        /// Provides enhanced error handling with screenshots and detailed messages.
        /// </summary>
        /// <param name="driver">The WebDriver instance</param>
        /// <param name="pattern">Regular expression pattern to match link text</param>
        /// <param name="searchContext">Optional context to search within. If null, searches entire page</param>
        /// <returns>True if link was found and clicked successfully, false otherwise</returns>
        public static bool BaseClickLinkByPattern(
            this IWebDriver driver,
            string pattern,
            IWebElement? searchContext = null)
        {
            var container = searchContext ?? driver.BaseFindElement(By.TagName("body"));
            var links = container.FindElements(By.TagName("a"));
            var regex = new Regex(pattern, RegexOptions.Compiled);
            var link = links.FirstOrDefault(l => regex.IsMatch(l.Text.Trim()));

            if (link == null)
            {
                var path = BrowserContext.Instance.CaptureFailureScreenshot($"link_pattern_not_found_{pattern}");
                TestContext.WriteLine($"Could not find link matching pattern '{pattern}'. Screenshot: {path}");
                return false;
            }

            try
            {
                link.Click();
                return true;
            }
            catch (WebDriverException ex)
            {
                var path = BrowserContext.Instance.CaptureFailureScreenshot($"link_click_failed_{pattern}");
                TestContext.WriteLine($"Failed to click link matching pattern '{pattern}'. Error: {ex.Message}. Screenshot: {path}");
                return false;
            }
        }

        #endregion
    }
}
