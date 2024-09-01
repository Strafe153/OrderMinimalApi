namespace MinimalApi.Configurations.Models;

public class OpenIddictOptions
{
    public const string SectionName = "OpenIddict";

    public required string TokenEndpoint { get; init; }
    public required string ClientId { get; init; }
    public required string ClientSecret { get; init; }
    public required string[] Permissions { get; init; }
    public required string DisplayName { get; set; }
}
