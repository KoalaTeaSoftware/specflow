using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SpecflowCore.Tests.Fixtures;
using NUnit.Framework;

namespace SpecflowCore.Tests.POM
{
    public static class BaseLinkAssertions
    {
        /// <summary>
        /// Finds a link element whose text matches the given pattern
        /// </summary>
        /// <param name="driver">The WebDriver instance</param>
        /// <param name="pattern">Regular expression pattern to match against link text</param>
        /// <param name="searchContext">Optional element to search within. If null, uses DefaultContext</param>
        /// <returns>The first matching link element, or null if none found</returns>
        public static IWebElement FindLinkWithText(
            this IWebDriver driver,
            string pattern,
            IWebElement? searchContext = null)
        {
            var regex = new Regex(pattern);
            var container = searchContext ?? driver.FindElement(BasePageLocators.Elements.DefaultContext);
            
            return container.FindElements(By.TagName("a"))
                          .FirstOrDefault(link => !string.IsNullOrEmpty(link.Text) && 
                                                regex.IsMatch(link.Text));
        }

        /// <summary>
        /// Checks if a link element's destination is accessible via HTTP
        /// </summary>
        /// <param name="driver">The WebDriver instance</param>
        /// <param name="link">The link element to check</param>
        /// <param name="timeoutSeconds">Optional timeout in seconds. Default is from test configuration</param>
        /// <returns>True if the link destination returns HTTP 200-299, false otherwise</returns>
        public static async Task<bool> IsLinkAccessible(
            this IWebDriver driver,
            IWebElement link,
            int timeoutSeconds = TestConfiguration.Timeouts.DefaultWaitSeconds)
        {
            using var client = new HttpClient { Timeout = TimeSpan.FromSeconds(timeoutSeconds) };
            var href = link.GetAttribute("href");
            var elementHtml = link.GetAttribute("outerHTML");

            if (string.IsNullOrEmpty(href))
            {
                TestContext.WriteLine($"Link has no href attribute or it is empty. Element: {elementHtml}");
                return false;
            }

            try
            {
                var request = new HttpRequestMessage(HttpMethod.Head, href);
                var response = await client.SendAsync(request);
                
                if (!response.IsSuccessStatusCode)
                {
                    TestContext.WriteLine($"Link returned HTTP {(int)response.StatusCode} {response.StatusCode}. Element: {elementHtml}");
                    return false;
                }
                return true;
            }
            catch (HttpRequestException ex)
            {
                TestContext.WriteLine($"Network error checking link: {ex.Message}. Element: {elementHtml}");
                return false;
            }
            catch (TaskCanceledException)
            {
                TestContext.WriteLine($"Timeout after {timeoutSeconds}s checking link. Element: {elementHtml}");
                return false;
            }
            catch (ArgumentException ex)
            {
                TestContext.WriteLine($"Invalid URL: {ex.Message}. Element: {elementHtml}");
                return false;
            }
            catch (Exception ex)
            {
                TestContext.WriteLine($"Unexpected error: {ex.GetType().Name} - {ex.Message}. Element: {elementHtml}");
                return false;
            }
        }

        /// <summary>
        /// Lists all link elements within a given context
        /// </summary>
        /// <param name="driver">The WebDriver instance</param>
        /// <param name="searchContext">Optional element to search within. If null, uses DefaultContext</param>
        /// <returns>List of all link elements in the context</returns>
        public static IReadOnlyList<IWebElement> ListLinks(
            this IWebDriver driver,
            IWebElement? searchContext = null)
        {
            var container = searchContext ?? driver.FindElement(BasePageLocators.Elements.DefaultContext);
            return container.FindElements(By.TagName("a")).ToList();
        }

        /// <summary>
        /// Finds all inaccessible links within a given context
        /// </summary>
        /// <param name="driver">The WebDriver instance</param>
        /// <param name="searchContext">Optional element to search within. If null, uses DefaultContext</param>
        /// <param name="timeoutSeconds">Optional timeout in seconds for each link check. Default is from test configuration</param>
        /// <returns>Dictionary of broken links where key is the link element and value is its HTML representation</returns>
        /// <remarks>
        /// This method is useful for smoke tests and site health checks.
        /// It will check all links in parallel for better performance.
        /// Each broken link will have a detailed error message in the test output.
        /// </remarks>
        public static async Task<Dictionary<IWebElement, string>> ListBrokenLinks(
            this IWebDriver driver,
            IWebElement? searchContext = null,
            int timeoutSeconds = TestConfiguration.Timeouts.DefaultWaitSeconds)
        {
            var links = driver.ListLinks(searchContext);
            var brokenLinks = new Dictionary<IWebElement, string>();

            // Check all links in parallel for better performance
            var tasks = links.Select(async link =>
            {
                if (!await driver.IsLinkAccessible(link, timeoutSeconds))
                {
                    brokenLinks[link] = link.GetAttribute("outerHTML");
                }
            });

            await Task.WhenAll(tasks);
            
            if (brokenLinks.Any())
            {
                TestContext.WriteLine($"\nFound {brokenLinks.Count} broken links in total:");
                foreach (var html in brokenLinks.Values)
                {
                    TestContext.WriteLine($"- {html}");
                }
            }

            return brokenLinks;
        }
    }
}
