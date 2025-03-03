using TechTalk.SpecFlow;
using System;
using SpecflowCore.Tests.Support;

namespace SpecflowCore.Tests.Support
{
    [Binding]
    public class BrowserHooks
    {
        [AfterScenario]
        public void AfterScenario()
        {
            try
            {
                BrowserContext.Instance.Reset();
                Console.WriteLine("WebDriver reset successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to reset WebDriver: {ex.Message}");
            }
        }

        [AfterTestRun]
        public static void AfterTestRun()
        {
            try
            {
                BrowserContext.Instance.Dispose();
                Console.WriteLine("WebDriver disposed successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to dispose WebDriver: {ex.Message}");
            }
        }
    }
}
