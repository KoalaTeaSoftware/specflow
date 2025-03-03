using OpenQA.Selenium;
using FluentAssertions;
using SpecflowCore.Tests.Support;

namespace SpecflowCore.Tests.POM
{
    /// <summary>
    /// Provides high-level page interaction methods that combine multiple basic actions.
    /// These methods represent common workflows and complex interactions.
    /// </summary>
    public static class BasePageActions
    {
        /// <summary>
        /// Verifies that a page's main heading matches expected text
        /// </summary>
        /// <returns>True if heading matches, false otherwise</returns>
        public static bool VerifyMainHeading(this IWebDriver driver, string expectedText)
        {
            var heading = driver.WaitForElementToHaveText(BasePageLocators.Elements.MainHeading, expectedText);
            return heading != null;
        }

        /// <summary>
        /// Gets the current main heading text
        /// </summary>
        /// <returns>The heading text or null if not found</returns>
        public static string GetMainHeadingText(this IWebDriver driver)
        {
            var heading = driver.FindElement(BasePageLocators.Elements.MainHeading);
            return heading?.Text;
        }
    }
}
