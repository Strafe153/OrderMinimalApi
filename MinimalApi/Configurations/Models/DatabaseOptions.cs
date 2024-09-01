namespace MinimalApi.Configurations.Models;

public class DatabaseOptions
{
    public const string SectionName = "Database";

    public required string ConnectionString { get; init; }
    public required string DatabaseName { get; init; }
    public required string UsersCollectionName { get; init; }
    public required string OrdersCollectionName { get; init; }
}