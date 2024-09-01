using Microsoft.Extensions.Diagnostics.HealthChecks;
using MinimalApi.HttpClients;

namespace MinimalApi.HealthChecks;

public class SeqHealthCheck : IHealthCheck
{
    private readonly SeqClient _seqClient;

    public SeqHealthCheck(SeqClient seqClient)
    {
        _seqClient = seqClient;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            await _seqClient.CheckSeqConnectionAsync(cancellationToken);
            return HealthCheckResult.Healthy();
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy(exception: ex);
        }
    }
}
