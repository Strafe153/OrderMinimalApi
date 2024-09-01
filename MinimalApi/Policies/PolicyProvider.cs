using Polly;
using Polly.Retry;
using RestSharp;

namespace MinimalApi.Policies;

public static class PolicyProvider
{
    public static AsyncRetryPolicy<RestResponse> WaitAndRetryPolicy => Policy
        .HandleResult<RestResponse>(r => !r.IsSuccessStatusCode)
        .Or<HttpRequestException>()
        .Or<TimeoutException>()
        .WaitAndRetryAsync(3, retry => TimeSpan.FromSeconds(Math.Pow(1.5, retry)));
}
