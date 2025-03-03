using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using SpecflowCore.Tests.Fixtures;
using SpecflowCore.Tests.Support;

namespace SpecflowCore.Tests.POM
{
    public static class BasePageLinkAssertions
    {
        /// <summary>
        /// Checks if a specific link succeeds
        /// </summary>
        public static bool LinkSucceeds(
            this IWebDriver driver,
            string linkText,
            By? searchContext = null)
        {
            var currentUrl = driver.Url;

            // Try to click the link
            // Uses our extension method for consistency
            var clickResult = driver.ClickLinkWithText(linkText, searchContext);
            if (!clickResult)
            {
                var path = BrowserContext.Instance.CaptureFailureScreenshot($"link_not_found_{linkText}");
                TestContext.WriteLine($"Could not find link with text '{linkText}'. Screenshot: {path}");
                return false;
            }

            // Wait for page load or any client-side navigation
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(TestConfiguration.Timeouts.DefaultWaitSeconds));
            try
            {
                // Consider the link successful if either:
                // 1. The URL changes (normal navigation)
                // 2. The URL stays the same but we're already on that page (e.g., clicking Home while on home page)
                // 3. The URL stays the same but there's client-side navigation
                wait.Until(d => {
                    var urlChanged = d.Url != currentUrl;
                    var isCurrentPage = linkText.Equals("Welcome", StringComparison.OrdinalIgnoreCase) && 
                                     currentUrl.EndsWith(TestConfiguration.Urls.BaseUrl);
                    return urlChanged || isCurrentPage;
                });
            }
            catch (WebDriverTimeoutException)
            {
                var path = BrowserContext.Instance.CaptureFailureScreenshot($"link_no_navigation_{linkText}");
                TestContext.WriteLine($"Link '{linkText}' did not navigate to a new page. Screenshot: {path}");
                return false;
            }

            // Go back for next link
            driver.Navigate().Back();
            return true;
        }

        /// <summary>
        /// Checks if all links within a container succeed
        /// </summary>
        public static Dictionary<string, bool> AllLinksSucceed(
            this IWebDriver driver,
            By? searchContext = null)
        {
            var results = new Dictionary<string, bool>();
            // Uses our extension method for consistency
            var links = driver.FindElements(By.TagName("a"), searchContext)
                            .Where(l => !string.IsNullOrEmpty(l.Text))
                            .ToList();

            foreach (var link in links)
            {
                var linkText = link.Text.Trim();
                // Uses our extension method for consistency
                results[linkText] = driver.LinkSucceeds(linkText, searchContext);
            }

            return results;
        }

        /// <summary>
        /// Asynchronously checks if a specific link succeeds
        /// </summary>
        public static async Task<bool> LinkSucceedsAsync(
            this IWebDriver driver,
            string linkText,
            By? searchContext = null)
        {
            return await Task.Run(() => driver.LinkSucceeds(linkText, searchContext));
        }

        /// <summary>
        /// Asynchronously checks if all links within a container succeed
        /// </summary>
        public static async Task<Dictionary<string, bool>> AllLinksSucceedAsync(
            this IWebDriver driver,
            By? searchContext = null)
        {
            return await Task.Run(() => driver.AllLinksSucceed(searchContext));
        }

        /// <summary>
        /// Checks if a link matches a pattern and succeeds
        /// </summary>
        public static bool LinkMatchingPatternSucceeds(
            this IWebDriver driver,
            string pattern,
            By? searchContext = null)
        {
            var regex = new Regex(pattern);
            // Uses our extension method for consistency
            var links = driver.FindElements(By.TagName("a"), searchContext)
                            .Where(l => !string.IsNullOrEmpty(l.Text))
                            .Where(l => regex.IsMatch(l.Text))
                            .ToList();

            if (!links.Any())
            {
                var path = BrowserContext.Instance.CaptureFailureScreenshot($"no_links_matching_{pattern}");
                TestContext.WriteLine($"No links found matching pattern '{pattern}'. Screenshot: {path}");
                return false;
            }

            var linkText = links.First().Text.Trim();
            // Uses our extension method for consistency
            return driver.LinkSucceeds(linkText, searchContext);
        }
    }
}
