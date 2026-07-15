namespace Api.HealthChecks.Seq;

public class SeqClient(HttpClient client)
{
    public Task CheckHealthAsync(CancellationToken cancellationToken)
    {
        var url = $"{client.BaseAddress}/connect/token";
        return client.PostAsync(url, JsonContent.Create("{}"), cancellationToken);
    }
}
