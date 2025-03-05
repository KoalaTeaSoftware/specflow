using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using SpecflowCore.Tests.Support;

namespace SpecflowCore.Tests.POM
{
    /// <summary>
    /// Provides methods for waiting and timing-related actions.
    /// These methods handle dynamic element loading and state changes.
    /// </summary>
    public static class BaseWaitActions
    {
        /// <summary>
        /// Waits for an element to be present in the DOM and visible.
        /// Uses enhanced error handling with screenshots.
        /// </summary>
        /// <param name="driver">The WebDriver instance</param>
        /// <param name="locator">The By locator to find the element</param>
        /// <param name="timeoutSeconds">Maximum time to wait in seconds</param>
        /// <returns>The found IWebElement if successful, null otherwise</returns>
        public static IWebElement? BaseWaitForElement(
            this IWebDriver driver,
            By locator,
            int timeoutSeconds = TestConfiguration.Timeouts.DefaultWaitSeconds)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutSeconds));
            try
            {
                return wait.Until(d => d.FindElement(locator));
            }
            catch (WebDriverTimeoutException)
            {
                var path = BrowserContext.Instance.CaptureFailureScreenshot($"wait_timeout_{locator.ToString().Replace('/', '_')}");
                TestContext.WriteLine($"Timeout waiting for element with locator: {locator}. Screenshot: {path}");
                return null;
            }
        }

        /// <summary>
        /// Waits for an element to be present, visible, and have specific text content.
        /// Uses enhanced error handling with screenshots.
        /// </summary>
        /// <param name="driver">The WebDriver instance</param>
        /// <param name="locator">The By locator to find the element</param>
        /// <param name="expectedText">The text to wait for</param>
        /// <param name="timeoutSeconds">Maximum time to wait in seconds</param>
        /// <returns>The found IWebElement if successful with matching text, null otherwise</returns>
        public static IWebElement? BaseWaitForElementToHaveText(
            this IWebDriver driver,
            By locator,
            string expectedText,
            int timeoutSeconds = TestConfiguration.Timeouts.DefaultWaitSeconds)
        {
            var element = BaseWaitForElement(driver, locator, timeoutSeconds);
            if (element == null) return null;

            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutSeconds));
            try
            {
                return wait.Until(d =>
                {
                    var text = element.Text?.Trim();
                    return text == expectedText ? element : null;
                });
            }
            catch (WebDriverTimeoutException)
            {
                var path = BrowserContext.Instance.CaptureFailureScreenshot($"text_timeout_{locator.ToString().Replace('/', '_')}");
                var actualText = element.Text?.Trim() ?? "no text";
                TestContext.WriteLine($"Timeout waiting for element text. Expected: '{expectedText}', Actual: '{actualText}'. Screenshot: {path}");
                return null;
            }
        }

        /// <summary>
        /// Waits for the URL to contain the specified string.
        /// </summary>
        /// <param name="driver">The WebDriver instance</param>
        /// <param name="expectedUrl">The URL to wait for</param>
        /// <param name="timeoutSeconds">Maximum time to wait in seconds</param>
        /// <returns>True if the URL contains the expected string, false otherwise</returns>
        public static bool BaseWaitForUrl(
            this IWebDriver driver,
            string expectedUrl,
            int timeoutSeconds = TestConfiguration.Timeouts.DefaultWaitSeconds)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutSeconds));
            try
            {
                return wait.Until(d => d.Url.Contains(expectedUrl));
            }
            catch (WebDriverTimeoutException)
            {
                return false;
            }
        }
    }
}
