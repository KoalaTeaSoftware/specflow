using NUnit.Framework;
using OpenQA.Selenium;
using SpecflowCore.Tests.POM;
using SpecflowCore.Tests.Support;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;

namespace SpecflowCore.Tests.StepDefinitions
{
    [Binding]
    public class NavigationSteps
    {
        private readonly IWebDriver _driver;
        private readonly LinkVerifier _linkVerifier;

        public NavigationSteps()
        {
            _driver = BrowserContext.Instance.Driver;
            _linkVerifier = new LinkVerifier(_driver);
        }

        private IWebElement GetNavContainer()
        {
            var container = _driver.BaseWaitForElement(
                MainNavigationLocators.Elements.MainNavContainer,
                timeoutSeconds: TestConfiguration.Timeouts.DefaultWaitSeconds
            );

            if (container == null)
            {
                BrowserContext.Instance.CaptureFailureScreenshot("navigation_container_not_found");
                Assert.Fail("Could not find navigation container");
            }

            return container;
        }

        [Then(@"the main navigation contains only these links:")]
        public void ThenTheMainNavigationContainsOnlyTheseLinks(Table table)
        {
            var expectedLinks = table.Rows.Select(row => row["Link Text"]).ToList();
            var container = GetNavContainer();

            try
            {
                var links = container.FindElements(MainNavigationLocators.Elements.NavLinks);
                var actualLinks = links.Select(link => link.Text).ToList();

                Assert.That(actualLinks, Is.EquivalentTo(expectedLinks),
                    "Navigation links do not match expected list");
            }
            catch (AssertionException)
            {
                BrowserContext.Instance.CaptureFailureScreenshot("navigation_links_mismatch");
                throw;
            }
        }

        [Then(@"all main navigation links are accessible")]
        public async Task ThenAllMainNavigationLinksAreAccessible()
        {
            var container = GetNavContainer();
            var brokenLinks = await _linkVerifier.VerifyLinks(container);
            
            if (brokenLinks.Any())
            {
                BrowserContext.Instance.CaptureFailureScreenshot("broken_navigation_links");
                var errorMessage = string.Join("\n", brokenLinks.Select(kvp => $"{kvp.Key}: {kvp.Value}"));
                Assert.Fail($"Found {brokenLinks.Count} broken links:\n{errorMessage}");
            }
        }
    }
}
