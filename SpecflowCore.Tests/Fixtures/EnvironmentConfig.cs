namespace SpecflowCore.Tests.Fixture
{
    // Right now this does absolutely nothing, but maybe we can use it in the future
    public static class EnvironmentConfig
    {
        public static string TestEnvironment => Environment.GetEnvironmentVariable("TEST_ENV") ?? "staging";
        
        public static bool IsProduction => TestEnvironment.Equals("production", StringComparison.OrdinalIgnoreCase);
    }
}