using MinimalApi.Policies;
using RestSharp;

namespace MinimalApi.HttpClients;

public class SeqClient : IDisposable
{
	private readonly RestClient _restClient;

	public SeqClient(HttpClient httpClient)
	{
        _restClient = new(httpClient);
    }

    public Task<RestResponse> CheckSeqConnectionAsync(CancellationToken cancellationToken) =>
        PolicyProvider.WaitAndRetryPolicy.ExecuteAsync(
            () => _restClient.GetAsync(new(), cancellationToken));

    public void Dispose()
    {
        _restClient?.Dispose();
        GC.SuppressFinalize(this);
    }
}
