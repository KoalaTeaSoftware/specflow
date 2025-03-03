using OpenQA.Selenium;
using System.Text.RegularExpressions;
using SpecflowCore.Tests.Support;
using NUnit.Framework;

namespace SpecflowCore.Tests.POM
{
    /// <summary>
    /// Extension methods for managing browser windows and tabs.
    /// Provides functionality for switching between windows, finding specific tabs,
    /// and managing window state.
    /// </summary>
    public static class BaseWindowActions
    {
        /// <summary>
        /// Switches to a new window/tab that matches the given criteria
        /// </summary>
        /// <param name="driver">The WebDriver instance</param>
        /// <param name="titlePattern">Optional regex pattern to match the window title</param>
        /// <param name="urlPattern">Optional regex pattern to match the window URL</param>
        /// <param name="timeoutSeconds">How long to wait for the window to appear</param>
        /// <returns>True if switch was successful, false otherwise</returns>
        public static bool SwitchToWindow(
            this IWebDriver driver,
            string? titlePattern = null,
            string? urlPattern = null,
            int timeoutSeconds = 10)
        {
            var endTime = DateTime.Now.AddSeconds(timeoutSeconds);
            string originalWindow = driver.CurrentWindowHandle;

            while (DateTime.Now < endTime)
            {
                foreach (var handle in driver.WindowHandles)
                {
                    if (handle == originalWindow) continue;

                    driver.SwitchTo().Window(handle);

                    bool titleMatches = titlePattern == null || 
                        Regex.IsMatch(driver.Title, titlePattern, RegexOptions.IgnoreCase);
                    bool urlMatches = urlPattern == null || 
                        Regex.IsMatch(driver.Url, urlPattern, RegexOptions.IgnoreCase);

                    if (titleMatches && urlMatches)
                    {
                        return true;
                    }
                }

                Thread.Sleep(500); // Wait before checking again
            }

            // If no matching window found, switch back to original
            driver.SwitchTo().Window(originalWindow);
            var path = BrowserContext.Instance.CaptureFailureScreenshot($"window_not_found_{titlePattern ?? "any"}_{urlPattern ?? "any"}");
            TestContext.WriteLine($"Could not find window matching title '{titlePattern}' and URL '{urlPattern}'. Screenshot: {path}");
            return false;
        }

        /// <summary>
        /// Switches to the most recently opened window/tab
        /// </summary>
        /// <returns>True if switch was successful, false if no new window found</returns>
        public static bool SwitchToNewWindow(this IWebDriver driver, int timeoutSeconds = 10)
        {
            var endTime = DateTime.Now.AddSeconds(timeoutSeconds);
            string originalWindow = driver.CurrentWindowHandle;
            int originalCount = driver.WindowHandles.Count;

            while (DateTime.Now < endTime)
            {
                if (driver.WindowHandles.Count > originalCount)
                {
                    driver.SwitchTo().Window(driver.WindowHandles.Last());
                    return true;
                }
                Thread.Sleep(500);
            }

            var path = BrowserContext.Instance.CaptureFailureScreenshot("new_window_not_found");
            TestContext.WriteLine($"No new window appeared after {timeoutSeconds} seconds. Screenshot: {path}");
            return false;
        }

        /// <summary>
        /// Closes the current window/tab and switches back to the previous one
        /// </summary>
        public static void CloseCurrentWindow(this IWebDriver driver)
        {
            if (driver.WindowHandles.Count > 1)
            {
                driver.Close();
                driver.SwitchTo().Window(driver.WindowHandles.Last());
            }
        }

        /// <summary>
        /// Gets all window handles with their titles and URLs
        /// Useful for debugging window/tab issues
        /// </summary>
        public static IEnumerable<(string Handle, string Title, string Url)> GetWindowInfo(this IWebDriver driver)
        {
            var currentHandle = driver.CurrentWindowHandle;
            var windows = new List<(string, string, string)>();

            foreach (var handle in driver.WindowHandles)
            {
                driver.SwitchTo().Window(handle);
                windows.Add((handle, driver.Title, driver.Url));
            }

            // Switch back to original window
            driver.SwitchTo().Window(currentHandle);
            return windows;
        }
    }
}
