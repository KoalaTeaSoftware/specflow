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
        private readonly BasePage _currentPage;

        public CommonSteps()
        {
            _currentPage = BrowserContext.Instance.GetPage<HomePage>(); // Default to HomePage, but this could be any page
        }

        [Given(@"I navigate to the home page")]
        public void GivenINavigateToTheHomePage()
        {
            BrowserContext.Instance.Driver.Navigate().GoToUrl(TestConfiguration.Urls.HomePage);
        }

        [Then(@"main heading is ""(.*)""")]
        public void ThenMainHeadingIs(string expectedText)
        {
            var mainHeading = _currentPage.WaitForElementToHaveText(By.CssSelector("h1"), expectedText);
            
            if (mainHeading == null)
            {
                // Get the actual text to include in the error message
                var actualHeading = _currentPage.FindElement(By.CssSelector("h1"))?.Text ?? "no heading found";
                Assert.Fail($"Expected main heading to be '{expectedText}' but found '{actualHeading}'");
            }
        }
    }
}