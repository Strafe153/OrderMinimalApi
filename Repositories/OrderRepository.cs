using Microsoft.Extensions.Options;
using MongoDB.Driver;
using OrderMinimalApi.Models;

namespace OrderMinimalApi.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly IMongoCollection<Order> _orders;

    public OrderRepository(IOptions<OrderDatabaseSettings> orderDatabaseSettings)
    {
        var mongoClient = new MongoClient(orderDatabaseSettings.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(orderDatabaseSettings.Value.DatabaseName);

        _orders = mongoDatabase.GetCollection<Order>(orderDatabaseSettings.Value.OrdersCollectionName);
    }

    public async Task<IEnumerable<Order>> GetAllAsync(CancellationToken token = default)
    {
        var orders = await _orders.Find(_ => true).ToListAsync(token);
        return orders;
    }

    public async Task<Order?> GetByIdAsync(string id, CancellationToken token = default)
    {
        var order = await _orders.Find(o => o.Id == id).SingleOrDefaultAsync(token);
        return order;
    }

    public async Task CreateAsync(Order order)
    {
        await _orders.InsertOneAsync(order);
    }

    public async Task UpdateAsync(string id, Order newOrder)
    {
        await _orders.ReplaceOneAsync(o => o.Id == id, newOrder);
    }

    public async Task DeleteAsync(string id)
    {
        await _orders.DeleteOneAsync(o => o.Id == id);
    }
}
