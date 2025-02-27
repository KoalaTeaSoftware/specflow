using OpenQA.Selenium;
using SpecflowCore.Tests.POM;

namespace SpecflowCore.Tests.Support
{
    public class BrowserContext
    {
        private static BrowserContext? _instance;
        private static readonly object _lock = new object();
        
        private IWebDriver? _driver;
        private BasePage? _currentPage;

        private BrowserContext() { }

        public static BrowserContext Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        _instance ??= new BrowserContext();
                    }
                }
                return _instance;
            }
        }

        public IWebDriver Driver
        {
            get
            {
                if (_driver == null)
                {
                    _driver = WebDriverFactory.CreateDriver();
                }
                return _driver;
            }
        }

        public T GetPage<T>() where T : BasePage, new()
        {
            _currentPage = new T();
            return (T)_currentPage;
        }

        public void CleanupContext()
        {
            _driver?.Quit();
            _driver = null;
        }
    }
}