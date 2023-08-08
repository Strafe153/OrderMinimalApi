namespace Core.Shared;

public class OrderDatabaseSettings
{
    public string ConnectionString { get; set; } = default!;
    public string DatabaseName { get; set; } = default!;
    public string OrdersCollectionName { get; set; } = default!;
}
