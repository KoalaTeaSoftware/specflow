using OpenQA.Selenium;
using FluentAssertions;
using SpecflowCore.Tests.Support;

namespace SpecflowCore.Tests.POM
{
    /// <summary>
    /// Provides high-level page interaction methods that combine multiple basic actions.
    /// These methods represent common workflows and complex interactions.
    /// </summary>
    public class BasePageActions
    {
        private readonly IWebDriver _driver;

        public BasePageActions(IWebDriver driver)
        {
            _driver = driver;
        }

        /// <summary>
        /// Verifies that a page's main heading matches expected text
        /// </summary>
        /// <returns>True if heading matches, false otherwise</returns>
        public bool VerifyHeading(string expectedText)
        {
            var heading = _driver.BaseWaitForElementToHaveText(
                BasePageLocators.Elements.MainHeading,
                expectedText,
                timeoutSeconds: TestConfiguration.Timeouts.DefaultWaitSeconds
            );
            return heading != null;
        }

        /// <summary>
        /// Gets the current main heading text
        /// </summary>
        /// <returns>The heading text or null if not found</returns>
        public string GetHeadingText()
        {
            var heading = _driver.BaseFindElement(BasePageLocators.Elements.MainHeading);
            return heading?.Text?.Trim() ?? string.Empty;
        }
    }
}
