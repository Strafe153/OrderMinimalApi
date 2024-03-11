namespace Domain.Shared;

public class OrderDatabaseOptions
{
	public const string SectionName = "OrderDatabase";

	public string ConnectionString { get; set; } = default!;
	public string DatabaseName { get; set; } = default!;
	public string OrdersCollectionName { get; set; } = default!;
}
