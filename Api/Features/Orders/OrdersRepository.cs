using Api.Constants;
using Api.Entities;
using MongoDB.Driver;

namespace Api.Features.Orders;

// Exists solely as a wrapper for unit-testing, since extension methods on IMongoCollection cannot be mocked
public interface IOrdersRepository
{
	Task<IEnumerable<Order>> GetAllAsync(CancellationToken token);
	Task<Order?> GetByIdAsync(string id, CancellationToken token);
	Task CreateAsync(Order order);
	Task UpdateAsync(string id, Order newOrder);
	Task DeleteAsync(string id, CancellationToken token);
}

public class OrdersRepository(IMongoDatabase mongoDatabase) : IOrdersRepository
{
	private readonly IMongoCollection<Order> _ordersCollection =
        mongoDatabase.GetCollection<Order>(MongoCollectionNames.OrdersCollection);

	public async Task<IEnumerable<Order>> GetAllAsync(CancellationToken token) =>
		await _ordersCollection.Find(_ => true).ToListAsync(token);

	public async Task<Order?> GetByIdAsync(string id, CancellationToken token) =>
		await _ordersCollection.Find(o => o.Id == id).FirstOrDefaultAsync(token);

	public Task CreateAsync(Order order) => _ordersCollection.InsertOneAsync(order);

	public Task UpdateAsync(string id, Order order) =>
        _ordersCollection.ReplaceOneAsync(o => o.Id == id, order);

	public Task DeleteAsync(string id, CancellationToken token) =>
		_ordersCollection.DeleteOneAsync(o => o.Id == id, token);
}