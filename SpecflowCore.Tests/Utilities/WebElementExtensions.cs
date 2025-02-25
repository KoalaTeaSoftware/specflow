using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace SpecflowCore.Tests.Utilities
{
    public static class WebElementExtensions
    {
        public static IWebElement WaitForElement(this IWebDriver driver, By by, int timeoutInSeconds = 10)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(d => d.FindElement(by));
        }

        public static bool WaitForElementText(this IWebDriver driver, By by, string expectedText, int timeoutInSeconds = 10)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(d => 
            {
                var element = d.FindElement(by);
                return element.Text.Contains(expectedText);
            });
        }

        public static IWebElement WaitForElementBelow(this IWebDriver driver, By childLocator, By parentLocator, int timeoutInSeconds = 10)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(d => 
            {
                var parent = d.FindElement(parentLocator);
                return parent.FindElement(childLocator);
            });
        }
    }
}