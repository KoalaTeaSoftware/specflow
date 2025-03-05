using NUnit.Framework;
using OpenQA.Selenium;
using SpecflowCore.Tests.POM;
using SpecflowCore.Tests.Support;
using TechTalk.SpecFlow;

namespace SpecflowCore.Tests.StepDefinitions
{
    [Binding]
    public class CommonSteps
    {
        private IWebDriver _driver;

        [BeforeScenario]
        public void BeforeScenario()
        {
            BrowserContext.Instance.Reset();
            _driver = BrowserContext.Instance.Driver;
        }

        [Given(@"The browser opens the home page")]
        public void GivenTheBrowserOpensTheHomePage()
        {
            _driver.Navigate().GoToUrl(TestConfiguration.Urls.HomePage);
        }

        [Then(@"The main heading is ""(.*)""")]
        public void ThenTheMainHeadingIs(string expectedText)
        {
            var heading = _driver.BaseWaitForElementToHaveText(
                BasePageLocators.Elements.MainHeading, 
                expectedText,
                timeoutSeconds: TestConfiguration.Timeouts.DefaultWaitSeconds);

            if (heading == null)
            {
                var actualText = _driver.BaseGetElementText(BasePageLocators.Elements.MainHeading) ?? "no heading found";
                Assert.Fail($"Expected heading to be '{expectedText}' but found '{actualText}'");
            }
        }
    }
}