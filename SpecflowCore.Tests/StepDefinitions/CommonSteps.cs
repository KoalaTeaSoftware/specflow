using OpenQA.Selenium;
using SpecflowCore.Tests.Support;
using TechTalk.SpecFlow;
using SpecflowCore.Tests.Fixtures;
using NUnit.Framework;
using SpecflowCore.Tests.POM;
using System.Text.RegularExpressions;
using FluentAssertions;

namespace SpecflowCore.Tests.StepDefinitions
{
    [Binding]
    public class CommonSteps
    {
        private readonly HomePage _homePage;
        private readonly IWebDriver _driver;

        public CommonSteps()
        {
            _homePage = BrowserContext.Instance.GetPage<HomePage>();
            _driver = BrowserContext.Instance.Driver;
        }

        [Given(@"The browser opens the home page")]
        public void BrowserOpensHomePage()
        {
            BrowserContext.Instance.Driver.Navigate().GoToUrl(TestConfiguration.Urls.HomePage);
        }

        [Then(@"The main heading is ""(.*)""")]
        public void TheMainHeadingIs(string expectedText)
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

        [Then(@"the newest tab is brought forward")]
        public void ThenTheNewestTabIsBroughtForward()
        {
            _driver.SwitchToNewWindow().Should().BeTrue("Expected a new browser tab to be available");
        }

        [Then(@"the ""(.*)"" tab is brought forward")]
        public void ThenTheTabIsBroughtForward(string pageName)
        {
            // Try both title and URL-based matching for flexibility
            var titlePattern = Regex.Escape(pageName);
            var urlPattern = Regex.Escape(pageName.ToLower().Replace(" ", "-"));
            
            _driver.SwitchToWindow(titlePattern: titlePattern, urlPattern: urlPattern)
                .Should().BeTrue($"Expected to find tab for {pageName}");
        }

        [Then(@"the ""(.*)"" page is displayed")]
        public void ThenIShouldSeeThePage(string pageName)
        {
            // Try both title and URL-based matching for flexibility
            var titlePattern = Regex.Escape(pageName);
            var urlPattern = Regex.Escape(pageName.ToLower().Replace(" ", "-"));
            
            _driver.SwitchToWindow(titlePattern: titlePattern, urlPattern: urlPattern)
                .Should().BeTrue($"Expected to see the {pageName} page");
        }

        [Then(@"the ""(.*)"" page is displayed again")]
        public void ThenIShouldBeBackOnThePage(string pageName)
        {
            var titlePattern = Regex.Escape(pageName);
            var urlPattern = Regex.Escape(pageName.ToLower().Replace(" ", "-"));
            
            _driver.SwitchToWindow(titlePattern: titlePattern, urlPattern: urlPattern)
                .Should().BeTrue($"Expected to be back on the {pageName} page");
        }

        [When(@"the current tab is closed")]
        public void WhenTheCurrentTabIsClosed()
        {
            _driver.CloseCurrentWindow();
        }

        [When(@"The link with text ""(.*)"" is clicked")]
        public void WhenTheLinkWithTextIsClicked(string linkText)
        {
            _driver.ClickLinkWithText(linkText)
                .Should().BeTrue($"Expected to find and click link with text '{linkText}'");
        }

        [When(@"The link containing text ""(.*)"" is clicked")]
        public void WhenTheLinkContainingTextIsClicked(string partialLinkText)
        {
            _driver.ClickLinkContainingText(partialLinkText)
                .Should().BeTrue($"Expected to find and click link containing text '{partialLinkText}'");
        }
    }
}