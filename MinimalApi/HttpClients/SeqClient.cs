using MinimalApi.Policies;

namespace MinimalApi.HttpClients;

public class SeqClient
{
	private readonly HttpClient _httpClient;

	public SeqClient(HttpClient httpClient)
	{
		_httpClient = httpClient;
	}

	public Task CheckSeqHealth() =>
		PolicyProvider.WaitRetryPolicy.ExecuteAsync(
			() => _httpClient.GetAsync(_httpClient.BaseAddress));
}
