using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SpecflowCore.Tests.Support;
using SpecflowCore.Tests.Fixtures;
using NUnit.Framework;

namespace SpecflowCore.Tests.POM
{
    public abstract class BasePage
    {
        public IWebDriver Driver => BrowserContext.Instance.Driver;
        private readonly By _defaultSearchContext = By.TagName("body");

        public IWebElement WaitForElement(By childLocator, int timeoutSeconds = TestConfiguration.Timeouts.DefaultWaitSeconds)
        {
            return WaitForElement(childLocator, _defaultSearchContext, timeoutSeconds);
        }

        public IWebElement WaitForElement(By childLocator, By searchContext, int timeoutSeconds = TestConfiguration.Timeouts.DefaultWaitSeconds)
        {
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(timeoutSeconds));
            return wait.Until(d => {
                var context = d.FindElement(searchContext);
                return context.FindElement(childLocator);
            });
        }

        public IWebElement WaitForElementToHaveText(By locator, string expectedText, int timeoutSeconds = TestConfiguration.Timeouts.DefaultWaitSeconds)
        {
            return WaitForElementToHaveText(locator, _defaultSearchContext, expectedText, timeoutSeconds);
        }

        public IWebElement WaitForElementToHaveText(By locator, By searchContext, string expectedText, int timeoutSeconds = TestConfiguration.Timeouts.DefaultWaitSeconds)
        {
            try
            {
                var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(timeoutSeconds));
                return wait.Until(d => {
                    var element = FindElement(locator, searchContext);
                    return (element != null && element.Text.Contains(expectedText)) ? element : null;
                });
            }
            catch (WebDriverTimeoutException)
            {
                NUnit.Framework.TestContext.WriteLine($"Timeout waiting for element '{locator}' to have text '{expectedText}' within parent context '{searchContext}'. Element was not found or text did not match.");
                return null;
            }
        }

        public IWebElement WaitForElementWithin(By childLocator, By parentContext, int timeoutSeconds = 8)
        {
            try
            {
                var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(timeoutSeconds));
                return wait.Until(d => FindElement(childLocator, parentContext));
            }
            catch (WebDriverTimeoutException)
            {
                NUnit.Framework.TestContext.WriteLine($"Timeout waiting for element '{childLocator}' within parent context '{parentContext}'. Element was not found.");
                return null;
            }
        }

        public IWebElement FindElement(By locator)
        {
            return FindElement(locator, _defaultSearchContext);
        }

        public IWebElement FindElement(By locator, By searchContext)
        {
            try
            {
                var context = Driver.FindElement(searchContext);
                return context.FindElement(locator);
            }
            catch (NoSuchElementException)
            {
                NUnit.Framework.TestContext.WriteLine($"Failed to find element '{locator}' within parent context '{searchContext}'.");
                return null;
            }
        }

        public void Click(By locator)
        {
            Click(locator, _defaultSearchContext);
        }

        public void Click(By locator, By searchContext)
        {
            FindElement(locator, searchContext)?.Click();
        }

        public void Type(By locator, string text)
        {
            Type(locator, text, _defaultSearchContext);
        }

        public void Type(By locator, string text, By searchContext)
        {
            FindElement(locator, searchContext)?.SendKeys(text);
        }
    }
}