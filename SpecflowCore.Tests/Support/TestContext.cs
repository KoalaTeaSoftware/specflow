using System;
using System.IO;
using TechTalk.SpecFlow;

namespace SpecflowCore.Tests.Support
{
    public static class TestRunContext
    {
        private static readonly string _runTimestamp;
        private static readonly string _testResultsPath;
        private static readonly string _screenshotsPath;
        private static readonly string _currentRunPath;
        private static ScenarioContext? _currentScenarioContext;

        public static string RunTimestamp => _runTimestamp;
        public static string TestResultsPath => _testResultsPath;
        public static string ScreenshotsPath => _screenshotsPath;
        public static string CurrentRunPath => _currentRunPath;
        public static ScenarioContext? CurrentScenarioContext
        {
            get => _currentScenarioContext;
            set => _currentScenarioContext = value;
        }

        static TestRunContext()
        {
            try
            {
                // Get or generate the timestamp once for the entire test run
                _runTimestamp = Environment.GetEnvironmentVariable("TEST_RUN_TIMESTAMP") ?? DateTime.Now.ToString("yyyyMMdd_HHmmss");
                
                // Set up the test results path
                _testResultsPath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "..", "TestResults"));
                
                // Set up the current run and screenshots paths
                _currentRunPath = Path.Combine(_testResultsPath, _runTimestamp);
                _screenshotsPath = Path.Combine(_currentRunPath, "Screenshots");

                // Ensure the test results and screenshots directories exist
                Directory.CreateDirectory(_currentRunPath);
                Directory.CreateDirectory(_screenshotsPath);

                Console.WriteLine($"Test run started at: {_runTimestamp}");
                Console.WriteLine($"Test results will be saved to: {_currentRunPath}");
                Console.WriteLine($"Screenshots will be saved to: {_screenshotsPath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to initialize TestRunContext: {ex.Message}");
                throw;
            }
        }
    }
}
