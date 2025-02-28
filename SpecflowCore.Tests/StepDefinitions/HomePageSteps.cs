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

       
    }
}