using Polly;
using Polly.Retry;
using System.Net.Http;

namespace Collette.Utilities
{
    internal class PollyPolicy<T>
    {
        private const int MaxRetries = 3;
        private readonly AsyncRetryPolicy<T> _retryPolicy;

        internal PollyPolicy()
        {
            _retryPolicy = Policy<T>.Handle<HttpRequestException>().RetryAsync(retryCount: MaxRetries);
        }

        public AsyncRetryPolicy<T> RetryPolicy => _retryPolicy;

    }
}
