using OpenQA.Selenium;

namespace SpecflowCore.Tests.POM
{
    public  class HomePage : BasePage
    {
        // explicitly stating this because some sites use h2 to be the page's title
        public static readonly By PageTitle = By.CssSelector("h1");
    }
}