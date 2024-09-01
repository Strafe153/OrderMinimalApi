using Domain.Entities;

namespace Domain.Interfaces.Repositories;

public interface IOrdersRepository
{
	Task<IEnumerable<Order>> GetAllAsync(CancellationToken token);
	Task<Order?> GetByIdAsync(string id, CancellationToken token);
	Task CreateAsync(Order order);
	Task UpdateAsync(string id, Order newOrder);
	Task DeleteAsync(string id);
}
