using OpenQA.Selenium;
using SpecflowCore.Tests.Fixtures;

namespace SpecflowCore.Tests.POM
{
    /// <summary>
    /// Defines the structure and elements specific to the Home page.
    /// </summary>
    public static class HomePage
    {
        public static void NavigateToHomePage(this IWebDriver driver)
        {
            driver.Navigate().GoToUrl(TestConfiguration.Urls.BaseUrl);
        }
    }
}