using Microsoft.Extensions.Options;
using MinimalApi.Entities;
using MinimalApi.Repositories.Abstractions;
using MinimalApi.Shared;
using MongoDB.Driver;

namespace MinimalApi.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly IMongoCollection<Order> _orders;

    public OrderRepository(IOptions<OrderDatabaseSettings> orderDatabaseSettings)
    {
        var mongoClient = new MongoClient(orderDatabaseSettings.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(orderDatabaseSettings.Value.DatabaseName);

        _orders = mongoDatabase.GetCollection<Order>(orderDatabaseSettings.Value.OrdersCollectionName);
    }

    public async Task<IEnumerable<Order>> GetAllAsync(CancellationToken token = default) =>
        await _orders.Find(_ => true).ToListAsync(token);

    public async Task<Order?> GetByIdAsync(string id, CancellationToken token = default) =>
        await _orders.Find(o => o.Id == id).FirstOrDefaultAsync(token);

    public async Task CreateAsync(Order order) =>
        await _orders.InsertOneAsync(order);

    public async Task UpdateAsync(string id, Order newOrder) =>
        await _orders.ReplaceOneAsync(o => o.Id == id, newOrder);

    public async Task DeleteAsync(string id) =>
        await _orders.DeleteOneAsync(o => o.Id == id);
}
