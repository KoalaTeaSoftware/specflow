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
        private readonly HomePage _homePage;

        public HomePageSteps()
        {
            _homePage = BrowserContext.Instance.GetPage<HomePage>();
        }

        [Then(@"[Tt]he [Hh]ome page loads")]
        public void ThenTheHomePageLoads()
        {
            try
            {
                Console.WriteLine($"Current URL: {_homePage.Driver.Url}");
                Console.WriteLine($"Waiting for element '{HomePage.PageTitle}' to have text 'Welcome'");
                
                var element = _homePage.WaitForElementToHaveText(HomePage.PageTitle, "Wel come");
                
                if (element == null)
                {
                    var pageSource = _homePage.Driver.PageSource;
                    Console.WriteLine($"Page source: {pageSource}");
                }
                
                Assert.That(element, Is.Not.Null, "Home page did not load - title element not found");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in ThenTheHomePageLoads: {ex.Message}");
                throw;
            }
        }
    }
}