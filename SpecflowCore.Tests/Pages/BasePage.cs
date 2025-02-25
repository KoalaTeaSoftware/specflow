using OpenQA.Selenium;
using SpecflowCore.Tests.Utilities;

namespace SpecflowCore.Tests.Pages
{
    public abstract class BasePage
    {
        protected readonly IWebDriver Driver;

        protected BasePage(IWebDriver driver)
        {
            Driver = driver;
        }

        protected IWebElement WaitForElement(By by, int timeoutInSeconds = 10)
        {
            return Driver.WaitForElement(by, timeoutInSeconds);
        }

        protected bool WaitForElementText(By by, string expectedText, int timeoutInSeconds = 10)
        {
            return Driver.WaitForElementText(by, expectedText, timeoutInSeconds);
        }
    }
}