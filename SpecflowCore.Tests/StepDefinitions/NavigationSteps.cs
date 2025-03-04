using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SpecflowCore.Tests.POM;
using SpecflowCore.Tests.Support;
using SpecflowCore.Tests.Fixtures;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using System.IO;
using NUnit.Framework;

namespace SpecflowCore.Tests.StepDefinitions
{
    [Binding]
    public class NavigationSteps
    {
        private readonly IWebDriver _driver;
        private readonly HttpClient _httpClient;

        public NavigationSteps()
        {
            _driver = BrowserContext.Instance.Driver;
            _httpClient = new HttpClient();
        }

        private bool IsOnHomePage()
        {
            return _driver.Url.TrimEnd('/') == TestConfiguration.Urls.BaseUrl.TrimEnd('/');
        }

        private bool CheckLink(IWebElement link)
        {
            string href = link.GetAttribute("href");
            string currentUrl = _driver.Url;
            
            // Special case for Welcome/Home link
            if (link.Text.Contains("Welcome", StringComparison.OrdinalIgnoreCase))
            {
                if (IsOnHomePage())
                {
                    // Already on home page, consider it valid
                    return true;
                }
            }

            if (!string.IsNullOrEmpty(href))
            {
                link.Click();
                System.Threading.Thread.Sleep(1000); // Wait for navigation
                
                // Check if navigation occurred
                string newUrl = _driver.Url;
                return newUrl != currentUrl || IsOnHomePage();
            }
            
            return false;
        }

        private async Task<bool> CheckLinkUrl(string url)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Head, url);
                var response = await _httpClient.SendAsync(request);
                return response.IsSuccessStatusCode;
            }
            catch (HttpRequestException)
            {
                return false;
            }
        }

        private bool CheckLinkAndHeading(IWebElement link, string expectedHeading)
        {
            string href = link.GetAttribute("href");
            if (string.IsNullOrEmpty(href))
            {
                return false;
            }

            // Special case for Welcome/Home link
            if (link.Text.Contains("Welcome", StringComparison.OrdinalIgnoreCase) && IsOnHomePage())
            {
                // Already on home page, verify heading
                try
                {
                    var heading = link.FindElement(By.CssSelector("h1, h2, h3, h4, h5, h6"));
                    var headingMatches = heading.Text.Contains(expectedHeading, StringComparison.OrdinalIgnoreCase);
                    if (!headingMatches)
                    {
                        var path = BrowserContext.Instance.CaptureFailureScreenshot($"{link.Text}_wrong_heading");
                        throw new Exception($"Navigation verification failed:\nLink [{link.Text}] page heading expected [{expectedHeading}] but got [{heading.Text}]\nFailure screenshot: {path}");
                    }
                    return headingMatches;
                }
                catch (NoSuchElementException)
                {
                    var path = BrowserContext.Instance.CaptureFailureScreenshot($"{link.Text}_heading_not_found");
                    throw new Exception($"Navigation verification failed:\nLink [{link.Text}] page heading expected [{expectedHeading}] but no heading was found\nFailure screenshot: {path}");
                }
            }

            link.Click();
            System.Threading.Thread.Sleep(1000); // Wait for navigation

            try
            {
                var heading = link.FindElement(By.CssSelector("h1, h2, h3, h4, h5, h6"));
                var headingMatches = heading.Text.Contains(expectedHeading, StringComparison.OrdinalIgnoreCase);
                if (!headingMatches)
                {
                    var path = BrowserContext.Instance.CaptureFailureScreenshot($"{link.Text}_wrong_heading");
                    throw new Exception($"Navigation verification failed:\nLink [{link.Text}] page heading expected [{expectedHeading}] but got [{heading.Text}]\nFailure screenshot: {path}");
                }
                return headingMatches;
            }
            catch (NoSuchElementException)
            {
                var path = BrowserContext.Instance.CaptureFailureScreenshot($"{link.Text}_no_heading_after_nav");
                throw new Exception($"Navigation verification failed:\nLink [{link.Text}] page heading expected [{expectedHeading}] but no heading was found\nFailure screenshot: {path}");
            }
        }

        [Then(@"the main navigation contains only these links:")]
        public void ThenTheMainNavigationContainsOnlyTheseLinks(Table expectedLinks)
        {
            // Wait for navigation bar to be present
            var navBar = _driver.WaitForElement(MainNavigationLocators.MainNav);
            Assert.That(navBar, Is.Not.Null, $"Unable to find element with locator {MainNavigationLocators.MainNav}");

            // Find links within the navigation bar context
            var links = navBar.FindElements(MainNavigationLocators.Links)
                             .Where(l => !string.IsNullOrEmpty(l.Text))
                             .ToList();
            var actualLinkTexts = links.Select(l => l.Text.Trim()).ToList();
            var expectedLinkTexts = expectedLinks.Rows.Select(r => r["Link Text"].Trim()).ToList();

            // Using TestContext for collecting all failures
            var failures = new List<string>();

            // Check for missing expected links
            var missingLinks = expectedLinkTexts.Except(actualLinkTexts).ToList();
            if (missingLinks.Any())
            {
                failures.Add($"Missing links: [{string.Join(", ", missingLinks)}]");
            }

            // Check for unexpected extra links
            var extraLinks = actualLinkTexts.Except(expectedLinkTexts).ToList();
            if (extraLinks.Any())
            {
                failures.Add($"Unexpected extra links: [{string.Join(", ", extraLinks)}]");
            }

            // Report all failures at once
            if (failures.Any())
            {
                var path = BrowserContext.Instance.CaptureFailureScreenshot("navigation_links_mismatch");
                Assert.Fail($"Navigation link check failed:\n{string.Join("\n", failures)}\nFailure screenshot: {path}");
            }
        }

        [Then(@"all main navigation links work")]
        public void ThenAllMainNavigationLinksWork()
        {
            var mainNav = _driver.WaitForElement(MainNavigationLocators.MainNav);
            Assert.That(mainNav, Is.Not.Null, $"Unable to find element with locator {MainNavigationLocators.MainNav}");

            var links = mainNav.FindElements(MainNavigationLocators.Links);
            var brokenLinks = new List<string>();

            foreach (var link in links)
            {
                if (!CheckLink(link))
                {
                    brokenLinks.Add(link.Text);
                }
            }

            if (brokenLinks.Any())
            {
                var path = BrowserContext.Instance.CaptureFailureScreenshot("broken_navigation_links");
                throw new Exception($"Navigation verification failed:\nBroken links: [{string.Join(", ", brokenLinks)}]\nFailure screenshot: {path}");
            }
        }

        [Then(@"all main navigation links are accessible")]
        public async Task ThenAllMainNavigationLinksAreAccessible()
        {
            var mainNav = _driver.WaitForElement(MainNavigationLocators.MainNav);
            Assert.That(mainNav, Is.Not.Null, $"Unable to find element with locator {MainNavigationLocators.MainNav}");

            var brokenLinks = await _driver.ListBrokenLinks(mainNav);
            Assert.That(brokenLinks, Is.Empty, "Navigation contains broken links. Check test output for details.");
        }

        [Then(@"clicking main navigation links leads to correct pages:")]
        public void ThenClickingMainNavigationLinksLeadsToCorrectPages(Table navigationMap)
        {
            var mainNav = _driver.WaitForElement(MainNavigationLocators.MainNav);
            Assert.That(mainNav, Is.Not.Null, "Navigation bar should be present");

            var links = mainNav.FindElements(MainNavigationLocators.Links);

            var failures = new List<string>();

            foreach (var row in navigationMap.Rows)
            {
                string linkText = row["Link Text"];
                string expectedHeading = row["Page Heading"];

                var link = links.FirstOrDefault(l => l.Text.Contains(linkText, StringComparison.OrdinalIgnoreCase));
                if (link == null)
                {
                    var path = BrowserContext.Instance.CaptureFailureScreenshot($"link_not_found_{linkText}");
                    var availableLinks = string.Join(", ", links.Select(l => $"'{l.Text}'"));
                    failures.Add($"Link '{linkText}' not found. Available links: {availableLinks}. Screenshot: {path}");
                    continue;
                }

                if (!CheckLinkAndHeading(link, expectedHeading))
                {
                    failures.Add($"Link '{linkText}' did not lead to page with heading '{expectedHeading}'");
                }

                // Go back to home page for next link
                _driver.Navigate().GoToUrl(TestConfiguration.Urls.HomePage);
                System.Threading.Thread.Sleep(1000); // Wait for navigation
                
                // Re-find elements as the page has been reloaded
                mainNav = _driver.WaitForElement(MainNavigationLocators.MainNav);
                links = mainNav.FindElements(MainNavigationLocators.Links);
            }

            if (failures.Any())
            {
                throw new Exception($"Navigation verification failed:\n{string.Join("\n", failures)}");
            }
        }
    }
}
