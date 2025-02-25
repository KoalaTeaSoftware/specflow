using NUnit.Framework;
using OpenQA.Selenium;
using SpecflowCore.Tests.Support;
using SpecflowCore.Tests.Utilities;
using TechTalk.SpecFlow;

namespace SpecflowCore.Tests.StepDefinitions
{
    [Binding]
    public class SampleSteps : TestBase
    {
        [Given(@"I navigate to ""(.*)""")]
        public void GivenINavigateTo(string url)
        {
            Driver.Navigate().GoToUrl(url);
        }

        [When(@"I wait for the page to load")]
        public void WhenIWaitForThePageToLoad()
        {
            // Basic wait for page load
            Driver.WaitForElement(By.TagName("body"));
        }

        [Then(@"I should see the page title ""(.*)""")]
        public void ThenIShouldSeeThePageTitle(string expectedTitle)
        {
            Assert.That(Driver.Title, Is.EqualTo(expectedTitle));
        }
    }
}