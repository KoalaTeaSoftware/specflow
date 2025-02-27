using NUnit.Framework;
using OpenQA.Selenium;
using SpecflowCore.Tests.POM;
using SpecflowCore.Tests.Support;
using TechTalk.SpecFlow;

namespace SpecflowCore.Tests.StepDefinitions
{
    [Binding]
    public class HomePageSteps
    {
        private readonly HomePage _homePage;

        public HomePageSteps()
        {
            _homePage = BrowserContext.Instance.GetPage<HomePage>();
        }

        [Then(@"[Tt]he [Hh]ome page loads")]
        public void ThenTheHomePageLoads()
        {
            // Wait until the title (as defined in the page model) exists
            // then assert that it contains the correct text
            var element = _homePage.WaitForElementToHaveText(HomePage.PageTitle, "Wessex Dramas");
            Assert.That(element, Is.Not.Null, "Home page did not load");
        }
    }
}