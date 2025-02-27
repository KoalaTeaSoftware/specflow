using OpenQA.Selenium;
using SpecflowCore.Tests.Support;
using TechTalk.SpecFlow;

namespace SpecflowCore.Tests.StepDefinitions
{
    [Binding]
    public class CommonSteps
    {
        [Given(@"I navigate to ""(.*)""")]
        public void GivenINavigateTo(string url)
        {
            BrowserContext.Instance.Driver.Navigate().GoToUrl(url);
        }
    }
}