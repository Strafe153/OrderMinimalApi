using OrderMinimalApi.Models;
using OrderMinimalApi.Repositories;

namespace OrderMinimalApi.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _repository;

        public OrderService(IOrderRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            var orders = await _repository.GetAllAsync();
            return orders;
        }

        public async Task<Order> GetByIdAsync(string id)
        {
            var order = await _repository.GetByIdAsync(id);

            if (order is null)
            {
                throw new NullReferenceException($"Order with id '{id}' not found");
            }

            return order;
        }

        public async Task CreateAsync(Order order)
        {
            await _repository.CreateAsync(order);
        }

        public async Task UpdateAsync(string id, Order newOrder)
        {
            try
            {
                await _repository.UpdateAsync(id, newOrder);
            }
            catch (Exception)
            {
                throw new NullReferenceException($"Order with id '{id}' not found");
            }
        }

        public async Task DeleteAsync(string id)
        {
            try
            {
                await _repository.DeleteAsync(id);
            }
            catch (Exception)
            {
                throw new NullReferenceException($"Order with id '{id}' not found");
            }
        }
    }
}
