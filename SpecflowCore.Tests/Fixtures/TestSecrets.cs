namespace SpecflowCore.Tests.Fixture
{
    public static class TestSecrets
    {
        // Load these from environment variables or a secure configuration source
        public static string GetSecret(string key) => Environment.GetEnvironmentVariable(key);
    }
}