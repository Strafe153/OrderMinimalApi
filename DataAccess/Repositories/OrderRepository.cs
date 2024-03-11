using Domain.Entities;
using Domain.Interfaces.Repositories;
using Domain.Shared;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace DataAccess.Repositories;

public class OrderRepository : IOrderRepository
{
	private readonly IMongoCollection<Order> _orders;

	public OrderRepository(IOptions<OrderDatabaseOptions> orderDatabaseSettings)
	{
		var mongoClient = new MongoClient(orderDatabaseSettings.Value.ConnectionString);
		var mongoDatabase = mongoClient.GetDatabase(orderDatabaseSettings.Value.DatabaseName);

		_orders = mongoDatabase.GetCollection<Order>(orderDatabaseSettings.Value.OrdersCollectionName);
	}

	public async Task<IEnumerable<Order>> GetAllAsync(CancellationToken token = default) =>
		await _orders.Find(_ => true).ToListAsync(token);

	public async Task<Order?> GetByIdAsync(string id, CancellationToken token = default) =>
		await _orders.Find(o => o.Id == id).FirstOrDefaultAsync(token);

	public Task CreateAsync(Order order) => _orders.InsertOneAsync(order);

	public Task UpdateAsync(string id, Order newOrder) => _orders.ReplaceOneAsync(o => o.Id == id, newOrder);

	public Task DeleteAsync(string id) => _orders.DeleteOneAsync(o => o.Id == id);
}
