using NUnit.Framework;
using OpenQA.Selenium;
using SpecflowCore.Tests.POM;
using SpecflowCore.Tests.Support;
using TechTalk.SpecFlow;
using System;

namespace SpecflowCore.Tests.StepDefinitions
{
    [Binding]
    public class HomePageSteps
    {
        private readonly IWebDriver _driver;

        public HomePageSteps()
        {
            _driver = BrowserContext.Instance.Driver;
        }

        [Given(@"I am on the home page")]
        public void GivenIAmOnTheHomePage()
        {
            HomePage.NavigateToHomePage(_driver);
        }
    }
}