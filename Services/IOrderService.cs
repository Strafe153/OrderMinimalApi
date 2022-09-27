using OrderMinimalApi.Models;

namespace OrderMinimalApi.Services;

public interface IOrderService
{
    Task<IEnumerable<Order>> GetAllAsync(CancellationToken token = default);
    Task<Order> GetByIdAsync(string id, CancellationToken token = default);
    Task CreateAsync(Order order);
    Task UpdateAsync(string id, Order newOrder);
    Task DeleteAsync(string id);
}
