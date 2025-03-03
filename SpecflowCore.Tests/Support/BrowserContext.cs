using OpenQA.Selenium;
using SpecflowCore.Tests.POM;

namespace SpecflowCore.Tests.Support
{
    /// <summary>
    /// Singleton class to manage browser context
    /// </summary>
    public class BrowserContext : IDisposable
    {
        private static BrowserContext _instance;
        private static readonly object Lock = new object();

        private BrowserContext()
        {
            Driver = WebDriverFactory.CreateDriver();
        }

        public IWebDriver Driver { get; private set; }

        public static BrowserContext Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (Lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new BrowserContext();
                        }
                    }
                }
                return _instance;
            }
        }

        public void Reset()
        {
            Driver?.Quit();
            Driver = WebDriverFactory.CreateDriver();
        }

        public void Dispose()
        {
            Driver?.Quit();
            Driver?.Dispose();
            Driver = null;
            _instance = null;
        }
    }
}