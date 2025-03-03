using OpenQA.Selenium;

namespace SpecflowCore.Tests.POM
{
    public static class MainNavigationLocators
    {
        // The main navigation container that holds the nav links
        public static readonly By MainNav = By.CssSelector("#myNavBar");
        
        // The links within the main navigation bar
        public static readonly By Links = By.CssSelector("#myNavBar .nav-item");
    }
}
