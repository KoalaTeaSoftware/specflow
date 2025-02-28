using System;
using System.Collections.Generic;

namespace SpecflowCore.Tests.Support
{
    public class TestResult
    {
        public TestResult()
        {
            Name = string.Empty;
            FullName = string.Empty;
            Message = string.Empty;
            StackTrace = string.Empty;
            Status = TestStatus.Failed;
        }

        public string Name { get; set; }
        public string FullName { get; set; }
        public TimeSpan Duration { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }
        public TestStatus Status { get; set; }
        public Dictionary<string, object> Properties { get; } = new Dictionary<string, object>();
    }

    public enum TestStatus
    {
        Passed,
        Failed,
        Skipped
    }
}
