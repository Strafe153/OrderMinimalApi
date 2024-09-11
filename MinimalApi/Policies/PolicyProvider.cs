using Flurl.Http;
using Polly;
using Polly.Retry;

namespace MinimalApi.Policies;

public static class PolicyProvider
{
    public static AsyncRetryPolicy FlurlWaitAndRetryPolicy =>
        Policy
            .Handle<FlurlHttpException>()
            .WaitAndRetryAsync(3, retry => TimeSpan.FromSeconds(Math.Pow(1.5, retry)));
}