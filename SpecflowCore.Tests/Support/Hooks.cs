using TechTalk.SpecFlow;

namespace SpecflowCore.Tests.Support
{
    [Binding]
    public class Hooks
    {
        [AfterScenario]
        public void AfterScenario()
        {
            BrowserContext.Instance.CleanupContext();
        }
    }
}