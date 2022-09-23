using OrderMinimalApi.Models;

namespace OrderMinimalApi.Repositories;

public interface IOrderRepository
{
    Task<IEnumerable<Order>> GetAllAsync();
    Task<Order?> GetByIdAsync(string id);
    Task CreateAsync(Order order);
    Task UpdateAsync(string id, Order newOrder);
    Task DeleteAsync(string id);
}
