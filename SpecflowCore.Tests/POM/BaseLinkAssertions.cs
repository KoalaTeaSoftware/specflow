using OpenQA.Selenium;
using SpecflowCore.Tests.Support;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;

namespace SpecflowCore.Tests.POM
{
    /// <summary>
    /// Utility class for verifying link accessibility
    /// </summary>
    public class LinkVerifier
    {
        private static readonly HttpClient _client = new HttpClient();
        private readonly IWebDriver _driver;

        public LinkVerifier(IWebDriver driver)
        {
            _driver = driver;
        }

        /// <summary>
        /// Verifies that all links in a container have valid URLs and are accessible
        /// </summary>
        /// <param name="searchContext">Optional context to search within</param>
        /// <returns>A dictionary of broken links and their issues</returns>
        public async Task<Dictionary<string, string>> VerifyLinks(IWebElement? searchContext = null)
        {
            var container = searchContext ?? _driver.BaseWaitForElement(
                BasePageLocators.Elements.DefaultContext,
                timeoutSeconds: TestConfiguration.Timeouts.DefaultWaitSeconds
            );

            if (container == null)
            {
                return new Dictionary<string, string> { { "Container", "Could not find container element" } };
            }

            var links = container.FindElements(By.TagName("a"));
            var brokenLinks = new Dictionary<string, string>();

            foreach (var link in links)
            {
                try
                {
                    var href = link.GetAttribute("href");
                    if (string.IsNullOrEmpty(href) || href.StartsWith("javascript:") || href.StartsWith("#"))
                    {
                        continue;
                    }

                    var response = await _client.GetAsync(href);
                    if (!response.IsSuccessStatusCode)
                    {
                        var elementHtml = link.GetAttribute("outerHTML");
                        brokenLinks.Add(href, $"Status code: {response.StatusCode}. Element: {elementHtml}");
                    }
                }
                catch (Exception ex)
                {
                    var elementHtml = link.GetAttribute("outerHTML");
                    brokenLinks.Add(link.GetAttribute("href") ?? "unknown",
                        $"Error: {ex.GetType().Name} - {ex.Message}. Element: {elementHtml}");
                }
            }

            return brokenLinks;
        }
    }
}
