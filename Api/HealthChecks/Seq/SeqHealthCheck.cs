using Api.Policies;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Api.HealthChecks.Seq;

public class SeqHealthCheck(SeqClient client) : IHealthCheck
{
    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        try
        {
            await PolicyProvider.WaitAndRetryPolicy.ExecuteAsync(() =>
                client.CheckHealthAsync(cancellationToken));

            return HealthCheckResult.Healthy();
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy(exception: ex);
        }
    }
}
