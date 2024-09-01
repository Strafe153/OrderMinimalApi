namespace MinimalApi.Configurations.Models;

public class SeqOptions
{
    public const string SectionName = "Seq";

    public required string ConnectionString { get; init; }
    public required string ApiKey { get; init; }
}
