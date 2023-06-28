using Mapster;
using Microsoft.Extensions.Caching.Memory;
using OrderMinimalApi.Dtos;
using OrderMinimalApi.Repositories;
using OrderMinimalApi.Shared;

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

        _memoryCacheEntryOptions = new MemoryCacheEntryOptions()
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(2),
            SlidingExpiration = TimeSpan.FromSeconds(10)
        };
    }

    public async Task<IEnumerable<OrderReadDto>> GetAllAsync(CancellationToken token = default)
    {
        var key = "orders";
        var orders = _memoryCache.Get<IEnumerable<Order>>(key);

        if (orders is null)
        {
            orders = await _repository.GetAllAsync(token);
            _memoryCache.Set(key, orders, _memoryCacheEntryOptions);
        }

        return orders.Adapt<IEnumerable<OrderReadDto>>();
    }

    public async Task<OrderReadDto> GetByIdAsync(string id, CancellationToken token = default)
    {
        var key = $"orders:{id}";
        var order = _memoryCache.Get<Order>(key);

        if (order is null)
        {
            order = await _repository.GetByIdAsync(id, token);

            if (order is null)
            {
                throw new NullReferenceException($"Order with id '{id}' not found");
            }

            _memoryCache.Set(key, order, _memoryCacheEntryOptions);
        }

        return order.Adapt<OrderReadDto>();
    }

    public async Task<OrderReadDto> CreateAsync(OrderCreateUpdateDto dto)
    {
        var order = dto.Adapt<Order>();
        await _repository.CreateAsync(order);

        return order.Adapt<OrderReadDto>();
    }

    public async Task UpdateAsync(string id, OrderCreateUpdateDto newOrderDto)
    {
        try
        {
            var readDto = await GetByIdAsync(id);
            newOrderDto.Adapt(readDto);

            var order = readDto.Adapt<Order>();
            await _repository.UpdateAsync(id, order);
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
