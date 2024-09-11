using Domain.Shared.Constants;
using Flurl.Http;
using Flurl.Http.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using MinimalApi.Policies;

namespace MinimalApi.HealthChecks;

public class SeqHealthCheck : IHealthCheck
{
    private readonly IFlurlClient _seqClient;

    public SeqHealthCheck(IFlurlClientCache clientCache)
    {
        _seqClient = clientCache.Get(FlurlConstants.SeqClient);
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            await PolicyProvider.FlurlWaitAndRetryPolicy.ExecuteAsync(
                () => _seqClient.Request().GetAsync(cancellationToken: cancellationToken));

            return HealthCheckResult.Healthy();
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy(exception: ex);
        }
    }
}
