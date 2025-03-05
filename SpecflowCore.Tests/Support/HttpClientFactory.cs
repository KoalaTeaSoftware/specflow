using System.Net.Http;

namespace SpecflowCore.Tests.Support
{
    /// <summary>
    /// Factory for creating and managing HttpClient instances.
    /// Uses a single shared HttpClient instance to avoid socket exhaustion.
    /// </summary>
    public static class HttpClientFactory
    {
        private static HttpClient? _client;
        private static readonly object _lock = new();

        /// <summary>
        /// Gets the shared HttpClient instance, creating it if needed.
        /// </summary>
        public static HttpClient Client
        {
            get
            {
                if (_client == null)
                {
                    lock (_lock)
                    {
                        _client ??= new HttpClient();
                    }
                }

                // Check if the client is disposed and recreate if needed
                try
                {
                    _ = _client.DefaultRequestHeaders;
                }
                catch (ObjectDisposedException)
                {
                    lock (_lock)
                    {
                        _client = new HttpClient();
                    }
                }

                return _client;
            }
        }
    }
}
