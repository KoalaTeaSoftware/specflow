using System;
using System.IO;
using System.Text;

namespace SpecflowCore.Tests.Support
{
    public class HtmlReportListener
    {
        private readonly StringBuilder _report = new StringBuilder();
        private int _totalTests = 0;
        private int _passedTests = 0;
        private int _failedTests = 0;

        public HtmlReportListener()
        {
            _report.AppendLine(@"<!DOCTYPE html>
<html>
<head>
    <title>Test Results</title>
    <style>
        body { font-family: Arial, sans-serif; line-height: 1.6; margin: 0; padding: 20px; background: #f5f5f5; }
        .container { max-width: 1200px; margin: 0 auto; }
        .header { background: white; padding: 20px; border-radius: 8px; margin-bottom: 20px; box-shadow: 0 2px 4px rgba(0,0,0,0.1); }
        .summary { display: grid; grid-template-columns: repeat(auto-fit, minmax(200px, 1fr)); gap: 20px; margin-bottom: 20px; }
        .summary-item { background: white; padding: 20px; border-radius: 8px; box-shadow: 0 2px 4px rgba(0,0,0,0.1); text-align: center; }
        .summary-item h3 { margin: 0 0 10px 0; color: #333; }
        .summary-item p { font-size: 24px; font-weight: bold; margin: 0; }
        .test { background: white; padding: 20px; margin-bottom: 20px; border-radius: 8px; box-shadow: 0 2px 4px rgba(0,0,0,0.1); }
        .test.failed { border-left: 5px solid #dc3545; }
        .test.passed { border-left: 5px solid #28a745; }
        .test-header { display: flex; justify-content: space-between; align-items: center; margin-bottom: 15px; }
        .test-title { margin: 0; color: #333; font-size: 18px; }
        .test-duration { color: #666; font-size: 14px; }
        .error { background: #fff5f5; padding: 15px; border-radius: 5px; margin-top: 10px; }
        .error h4 { color: #721c24; margin: 0 0 10px 0; }
        .error pre { margin: 10px 0; white-space: pre-wrap; background: #f8f9fa; padding: 10px; border-radius: 4px; }
        .screenshot { margin-top: 15px; }
        .screenshot h4 { margin: 0 0 10px 0; color: #333; }
        .screenshot img { max-width: 100%; height: auto; border-radius: 5px; box-shadow: 0 2px 4px rgba(0,0,0,0.1); }
        .badge { padding: 5px 10px; border-radius: 15px; font-size: 12px; font-weight: bold; }
        .badge.passed { background: #d4edda; color: #155724; }
        .badge.failed { background: #f8d7da; color: #721c24; }
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h1>Test Execution Report</h1>
            <p>Generated on: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + @"</p>
        </div>
        <div class='summary'>
            <div class='summary-item'>
                <h3>Total Tests</h3>
                <p id='total-tests'>0</p>
            </div>
            <div class='summary-item'>
                <h3>Passed Tests</h3>
                <p id='passed-tests' style='color: #28a745;'>0</p>
            </div>
            <div class='summary-item'>
                <h3>Failed Tests</h3>
                <p id='failed-tests' style='color: #dc3545;'>0</p>
            </div>
        </div>
        <div id='test-results'>");
        }

        public void OnTestStart(string scenarioTitle)
        {
            _totalTests++;
            Console.WriteLine($"Starting test: {scenarioTitle}");
        }

        public void OnTestPass(string scenarioTitle, TimeSpan duration)
        {
            _passedTests++;

            // Get the screenshot path from ScenarioContext
            var screenshotPath = TestRunContext.CurrentScenarioContext?.ContainsKey("LastScreenshotPath") == true
                ? TestRunContext.CurrentScenarioContext["LastScreenshotPath"]?.ToString()
                : null;

            var relativeScreenshotPath = !string.IsNullOrEmpty(screenshotPath)
                ? Path.GetRelativePath(TestRunContext.CurrentRunPath, screenshotPath)
                : null;

            _report.AppendLine($@"
            <div class='test passed'>
                <div class='test-header'>
                    <h3 class='test-title'>{scenarioTitle}</h3>
                    <span class='badge passed'>Passed</span>
                </div>
                <div class='test-duration'>Duration: {duration.TotalSeconds:F1} seconds</div>");

            if (!string.IsNullOrEmpty(relativeScreenshotPath))
            {
                _report.AppendLine($@"
                <div class='screenshot'>
                    <h4>Screenshot:</h4>
                    <img src='{relativeScreenshotPath}' alt='Test Screenshot'>
                </div>");
            }

            _report.AppendLine("</div>");
            Console.WriteLine($"Test passed: {scenarioTitle}");
        }

        public void OnTestFail(string scenarioTitle, TimeSpan duration, string errorMessage, string stackTrace, string screenshotPath)
        {
            _failedTests++;
            var relativeScreenshotPath = !string.IsNullOrEmpty(screenshotPath)
                ? Path.GetRelativePath(TestRunContext.CurrentRunPath, screenshotPath)
                : null;

            _report.AppendLine($@"
            <div class='test failed'>
                <div class='test-header'>
                    <h3 class='test-title'>{scenarioTitle}</h3>
                    <span class='badge failed'>Failed</span>
                </div>
                <div class='test-duration'>Duration: {duration.TotalSeconds:F1} seconds</div>
                <div class='error'>
                    <h4>Error Details:</h4>
                    <pre>{errorMessage}</pre>
                    <pre>{stackTrace}</pre>
                </div>");

            if (!string.IsNullOrEmpty(relativeScreenshotPath))
            {
                _report.AppendLine($@"
                <div class='screenshot'>
                    <h4>Screenshot:</h4>
                    <img src='{relativeScreenshotPath}' alt='Test Failure Screenshot'>
                </div>");
            }

            _report.AppendLine("</div>");
            Console.WriteLine($"Test failed: {scenarioTitle}");
        }

        public void SaveReport()
        {
            try
            {
                _report.AppendLine(@"
        </div>
        <script>
            document.getElementById('total-tests').textContent = '" + _totalTests + @"';
            document.getElementById('passed-tests').textContent = '" + _passedTests + @"';
            document.getElementById('failed-tests').textContent = '" + _failedTests + @"';
        </script>
    </div>
</body>
</html>");

                var reportPath = Path.Combine(TestRunContext.CurrentRunPath, "TestReport.html");
                File.WriteAllText(reportPath, _report.ToString());
                Console.WriteLine($"Test report saved to: {reportPath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to save test report: {ex.Message}");
            }
        }
    }
}
