using OpenQA.Selenium;

namespace SpecflowCore.Tests.POM
{
    /// <summary>
    /// Locators for the main navigation elements
    /// </summary>
    public static class MainNavigationLocators
    {
        public static class Elements
        {
            // Main navigation container - using ID as required
            public static readonly By MainNavContainer = By.Id("myNavBar");

            // Navigation links - using ID-based approach
            public static readonly By NavLinks = By.CssSelector("#myNavBar .nav-item");
        }
    }
}
