using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SpecflowCore.Tests.Fixtures;
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
            By? searchContext = null,
            int timeoutSeconds = TestConfiguration.Timeouts.DefaultWaitSeconds)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutSeconds));
            try
            {
                return wait.Until(d => d.FindElement(locator, searchContext));
            }
            catch (WebDriverTimeoutException)
            {
                TestContext.WriteLine($"Timeout waiting for element '{locator}' within parent context '{searchContext ?? BasePage.Elements.DefaultContext}'.");
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
            By? searchContext = null,
            int timeoutSeconds = TestConfiguration.Timeouts.DefaultWaitSeconds)
        {
            try
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutSeconds));
                return wait.Until(d =>
                {
                    var element = d.FindElement(locator, searchContext);
                    return (element != null && element.Text.Contains(expectedText)) ? element : null;
                });
            }
            catch (WebDriverTimeoutException)
            {
                var element = driver.FindElement(locator, searchContext);
                var actualText = element?.Text ?? "no text found";
                TestContext.WriteLine($"Timeout waiting for element '{locator}' to have text '{expectedText}'. Actual text was '{actualText}'");
                return null;
            }
        }
    }
}
