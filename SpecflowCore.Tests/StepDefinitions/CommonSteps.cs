using OpenQA.Selenium;
using SpecflowCore.Tests.Support;
using TechTalk.SpecFlow;
using SpecflowCore.Tests.Fixtures;
using NUnit.Framework;
using SpecflowCore.Tests.POM;

namespace SpecflowCore.Tests.StepDefinitions
{
    [Binding]
    public class CommonSteps
    {
        private readonly HomePage _homePage;

        public CommonSteps()
        {
            _homePage = BrowserContext.Instance.GetPage<HomePage>();
        }

        [Given(@"The browser opens the home page")]
        public void BrowserOpensHomePage()
        {
            BrowserContext.Instance.Driver.Navigate().GoToUrl(TestConfiguration.Urls.HomePage);
        }

        [Then(@"main heading is ""(.*)""")]
        public void MainHeadingIs(string expectedText)
        {
            var heading = _homePage.Driver.WaitForElementToHaveText(
                BasePage.Elements.MainHeading, 
                expectedText);

            if (heading == null)
            {
                var actualText = _homePage.Driver.GetText(BasePage.Elements.MainHeading) ?? "no heading found";
                Assert.Fail($"Expected main heading to be '{expectedText}' but found '{actualText}'");
            }
        }
    }
}