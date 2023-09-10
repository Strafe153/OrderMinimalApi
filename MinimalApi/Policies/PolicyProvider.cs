using Polly;
using Polly.Extensions.Http;
using Polly.Retry;

namespace MinimalApi.Policies;

public static class PolicyProvider
{
    public static AsyncRetryPolicy<HttpResponseMessage> WaitRetryPolicy => HttpPolicyExtensions
        .HandleTransientHttpError()
        .WaitAndRetryAsync(3, retryCount => TimeSpan.FromSeconds(Math.Pow(1.5, retryCount)));
}
