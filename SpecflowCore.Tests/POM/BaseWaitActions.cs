using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SpecflowCore.Tests.Fixtures;
using SpecflowCore.Tests.Support;
using NUnit.Framework;

namespace SpecflowCore.Tests.POM
{
    /// <summary>
    /// Provides methods for waiting and timing-related actions.
    /// These methods handle dynamic element loading and state changes.
    /// </summary>
    public static class BaseWaitActions
    {
        /// <summary>
        /// Waits for an element to be present and visible
        /// </summary>
        /// <param name="driver">The WebDriver instance</param>
        /// <param name="locator">The locator for the target element</param>
        /// <param name="searchContext">Optional context to search within</param>
        /// <param name="timeoutSeconds">How long to wait before timing out</param>
        /// <returns>The found element or null if timeout occurs</returns>
        public static IWebElement WaitForElement(
            this IWebDriver driver,
            By locator,
            IWebElement? searchContext = null,
            int timeoutSeconds = TestConfiguration.Timeouts.DefaultWaitSeconds)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutSeconds));
            try
            {
                return wait.Until(d => searchContext != null ? searchContext.FindElement(locator) : d.FindElement(locator));
            }
            catch (WebDriverTimeoutException)
            {
                var path = BrowserContext.Instance.CaptureFailureScreenshot($"element_not_found_{locator.ToString().Replace('/', '_')}");
                var contextDescription = searchContext != null ? "provided element context" : "page context";
                TestContext.WriteLine($"Timeout waiting for element '{locator}' within {contextDescription}. Screenshot: {path}");
                return null;
            }
        }

        /// <summary>
        /// Waits for an element to have specific text content
        /// </summary>
        /// <returns>The element if found with matching text, null otherwise</returns>
        public static IWebElement WaitForElementToHaveText(
            this IWebDriver driver,
            By locator,
            string expectedText,
            IWebElement? searchContext = null,
            int timeoutSeconds = TestConfiguration.Timeouts.DefaultWaitSeconds)
        {
            var element = WaitForElement(driver, locator, searchContext, timeoutSeconds);
            if (element == null) return null;

            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutSeconds));
            try
            {
                return wait.Until(d =>
                {
                    var text = element.Text;
                    return text.Contains(expectedText) ? element : null;
                });
            }
            catch (WebDriverTimeoutException)
            {
                var path = BrowserContext.Instance.CaptureFailureScreenshot($"element_wrong_text_{locator.ToString().Replace('/', '_')}");
                TestContext.WriteLine($"Timeout waiting for element '{locator}' to have text '{expectedText}'. Screenshot: {path}");
                return null;
            }
        }
    }
}
