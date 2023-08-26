using Microsoft.Extensions.Diagnostics.HealthChecks;
using MinimalApi.HttpClients;

namespace MinimalApi.HealthChecks;

public class SeqHealthCheck : IHealthCheck
{
    private readonly SeqClient _client;

    public SeqHealthCheck(SeqClient client)
    {
        _client = client;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            await _client.CheckSeqHealth();
            return HealthCheckResult.Healthy();
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy(exception: ex);
        }
    }
}
