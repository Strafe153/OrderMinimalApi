using Microsoft.Extensions.Caching.Memory;
using OrderMinimalApi.Models;
using OrderMinimalApi.Repositories;

namespace OrderMinimalApi.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _repository;
    private readonly IMemoryCache _memoryCache;
    private readonly MemoryCacheEntryOptions _memoryCacheEntryOptions;

    public OrderService(
        IOrderRepository repository,
        IMemoryCache memoryCache)
    {
        _repository = repository;
        _memoryCache = memoryCache;

        _memoryCacheEntryOptions = new()
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(2),
            SlidingExpiration = TimeSpan.FromSeconds(10)
        };
    }

    public async Task<IEnumerable<Order>> GetAllAsync()
    {
        string key = "orders";
        var orders = _memoryCache.Get<IEnumerable<Order>>(key);

        if (orders is null)
        {
            orders = await _repository.GetAllAsync();
            _memoryCache.Set(key, orders, _memoryCacheEntryOptions);
        }

        return orders;
    }

    public async Task<Order> GetByIdAsync(string id)
    {
        string key = $"orders:{id}";
        var order = _memoryCache.Get<Order>(key);

        if (order is null)
        {
            order = await _repository.GetByIdAsync(id);

            if (order is null)
            {
                throw new NullReferenceException($"Order with id '{id}' not found");
            }

            _memoryCache.Set(key, order, _memoryCacheEntryOptions);
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
