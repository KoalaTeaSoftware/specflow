namespace SpecflowCore.Tests.Support
{
    public static class TestConfiguration
    {
        public static class Timeouts
        {
            /// <summary>
            /// Default timeout for waiting operations in seconds
            /// </summary>
            public const int DefaultWaitSeconds = 10;

            /// <summary>
            /// Extended timeout for longer operations in seconds
            /// </summary>
            public const int ExtendedWaitSeconds = 30;

            /// <summary>
            /// Short timeout for quick checks in seconds
            /// </summary>
            public const int ShortWaitSeconds = 3;
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
