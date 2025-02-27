using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SpecflowCore.Tests.Support;

namespace SpecflowCore.Tests.POM
{
    public abstract class BasePage
    {
        protected IWebDriver Driver => BrowserContext.Instance.Driver;
        private readonly By _defaultSearchContext = By.TagName("body");

        // Change all protected to public
        public IWebElement WaitForElement(By childLocator, int timeoutSeconds = 10)
        {
            return WaitForElement(childLocator, _defaultSearchContext, timeoutSeconds);
        }

        public IWebElement WaitForElement(By childLocator, By searchContext, int timeoutSeconds = 10)
        {
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(timeoutSeconds));
            return wait.Until(d => {
                var context = d.FindElement(searchContext);
                return context.FindElement(childLocator);
            });
        }

        public IWebElement WaitForElementToHaveText(By locator, string expectedText, int timeoutSeconds = 8)
        {
            return WaitForElementToHaveText(locator, _defaultSearchContext, expectedText, timeoutSeconds);
        }

        public IWebElement WaitForElementToHaveText(By locator, By searchContext, string expectedText, int timeoutSeconds = 8)
        {
            try
            {
                var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(timeoutSeconds));
                // We have to use alambda hee because the wait needs something that it can repeatedly call
                return wait.Until(d => {
                    var element = FindElement(locator, searchContext);
                    return (element != null && element.Text.Contains(expectedText)) ? element : null;
                });
            }
            catch (WebDriverTimeoutException)
            {
                TestContext.WriteLine($"Timeout waiting for element '{locator}' to have text '{expectedText}' within parent context '{searchContext}'. Element was not found or text did not match.");
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
                TestContext.WriteLine($"Timeout waiting for element '{childLocator}' within parent context '{parentContext}'. Element was not found.");
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
                TestContext.WriteLine($"Failed to find element '{locator}' within parent context '{searchContext}'.");
                return null;
            }
        }

        public void Click(By locator)
        {
            Click(locator, _defaultSearchContext);
        }

        public void Click(By locator, By searchContext)
        {
            FindElement(locator, searchContext).Click();
        }

        public void Type(By locator, string text)
        {
            Type(locator, text, _defaultSearchContext);
        }

        public void Type(By locator, string text, By searchContext)
        {
            FindElement(locator, searchContext).SendKeys(text);
        }
    }
}