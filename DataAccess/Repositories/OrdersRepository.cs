using Domain.Entities;
using Domain.Interfaces.Repositories;
using MongoDB.Driver;

namespace DataAccess.Repositories;

public class OrdersRepository : IOrdersRepository
{
	private readonly IMongoCollection<Order> _ordersCollection;

	public OrdersRepository(IMongoDatabase mongoDatabase)
	{
		_ordersCollection = mongoDatabase.GetCollection<Order>(MongoCollectionNames.OrdersCollection);
	}

	public async Task<IEnumerable<Order>> GetAllAsync(CancellationToken token) =>
		await _ordersCollection.Find(_ => true).ToListAsync(token);

	public async Task<Order?> GetByIdAsync(string id, CancellationToken token) =>
		await _ordersCollection.Find(o => o.Id == id).FirstOrDefaultAsync(token);

	public Task CreateAsync(Order order) => _ordersCollection.InsertOneAsync(order);

	public Task UpdateAsync(string id, Order newOrder) => _ordersCollection.ReplaceOneAsync(o => o.Id == id, newOrder);

	public Task DeleteAsync(string id) => _ordersCollection.DeleteOneAsync(o => o.Id == id);
}
