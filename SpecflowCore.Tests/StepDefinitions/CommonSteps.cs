using OpenQA.Selenium;
using SpecflowCore.Tests.Support;
using TechTalk.SpecFlow;
using SpecflowCore.Tests.Fixtures;

namespace SpecflowCore.Tests.StepDefinitions
{
    [Binding]
    public class CommonSteps
    {
        [Given(@"I navigate to the home page")]
        public void GivenINavigateToTheHomePage()
        {
            BrowserContext.Instance.Driver.Navigate().GoToUrl(TestConfiguration.Urls.HomePage);
        }
    }
}