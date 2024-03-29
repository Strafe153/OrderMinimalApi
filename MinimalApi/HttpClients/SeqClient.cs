﻿using MinimalApi.Policies;

namespace MinimalApi.HttpClients;

public class SeqClient
{
    private readonly HttpClient _httpClient;

    public SeqClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task CheckSeqHealth() =>
        await PolicyProvider.WaitRetryPolicy.ExecuteAsync(
            async () => await _httpClient.GetAsync(_httpClient.BaseAddress));
}
