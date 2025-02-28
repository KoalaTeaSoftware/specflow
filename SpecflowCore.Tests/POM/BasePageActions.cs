using OpenQA.Selenium;

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
        public static bool VerifyMainHeading(this BasePage page, string expectedText)
        {
            var heading = page.Driver.WaitForElementToHaveText(BasePage.Elements.MainHeading, expectedText);
            return heading != null;
        }

        /// <summary>
        /// Gets the current main heading text
        /// </summary>
        /// <returns>The heading text or null if not found</returns>
        public static string GetMainHeadingText(this BasePage page)
        {
            var heading = page.Driver.FindElement(BasePage.Elements.MainHeading);
            return heading?.Text;
        }
    }
}
