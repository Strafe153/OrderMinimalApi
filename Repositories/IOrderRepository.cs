using OrderMinimalApi.Shared;

namespace OrderMinimalApi.Repositories;

public interface IOrderRepository
{
    Task<IEnumerable<Order>> GetAllAsync(CancellationToken token = default);
    Task<Order?> GetByIdAsync(string id, CancellationToken token = default);
    Task CreateAsync(Order order);
    Task UpdateAsync(string id, Order newOrder);
    Task DeleteAsync(string id);
}
