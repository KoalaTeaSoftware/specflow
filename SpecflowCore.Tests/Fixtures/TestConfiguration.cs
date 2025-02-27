namespace SpecflowCore.Tests.Fixtures
{
    public static class TestConfiguration
    {
        public static class Timeouts
        {
            public const int ShortWaitSeconds = 1;
            public const int DefaultWaitSeconds = ShortWaitSeconds * 2;
            public const int LongWaitSeconds = ShortWaitSeconds * 8;
        }

        public static class Urls
        {
            public const string BaseUrl = "https://wessexdramas.org";
            public const string HomePage = BaseUrl + "/";
        }

        public static class TestUsers
        {
            public static class Admin
            {
                public const string Username = "admin@wessexdramas.org";
                public const string Password = "PLACEHOLDER"; // Consider using secrets management
            }
            
            public static class StandardUser
            {
                public const string Username = "user@wessexdramas.org";
                public const string Password = "PLACEHOLDER";
            }
        }
    }
}