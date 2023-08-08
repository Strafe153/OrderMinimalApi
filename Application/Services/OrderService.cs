using Core.Dtos;
using Core.Entities;
using Core.Exceptions;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.Shared;
using Mapster;

namespace Core.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _repository;
    private readonly ICacheService _cacheService;
    private readonly CacheOptions _cacheOptions;

    public OrderService(
        IOrderRepository repository,
        ICacheService cacheService)
    {
        _repository = repository;
        _cacheService = cacheService;

        _cacheOptions = new CacheOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(2),
            SlidingExpiration = TimeSpan.FromSeconds(10)
        };
    }

    public async Task<IEnumerable<OrderReadDto>> GetAllAsync(CancellationToken token = default)
    {
        var key = "orders";
        var orders = _cacheService.Get<IEnumerable<Order>>(key);

        if (orders is null)
        {
            orders = await _repository.GetAllAsync(token);
            _cacheService.Set(key, orders, _cacheOptions);
        }

        return orders.Adapt<IEnumerable<OrderReadDto>>();
    }

    public async Task<OrderReadDto> GetByIdAsync(string id, CancellationToken token = default)
    {
        var key = $"orders:{id}";
        var order = _cacheService.Get<Order>(key);

        if (order is null)
        {
            order = await _repository.GetByIdAsync(id, token);

            if (order is null)
            {
                throw new NullReferenceException($"Order with id '{id}' not found.");
            }

            _cacheService.Set(key, order, _cacheOptions);
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
        var readDto = await GetByIdAsync(id);

        try
        {
            newOrderDto.Adapt(readDto);

            var order = readDto.Adapt<Order>();
            await _repository.UpdateAsync(id, order);
        }
        catch (Exception)
        {
            throw new OperationFailedException($"Failed to update an order with id={id}.");
        }
    }

    public async Task DeleteAsync(string id)
    {
        await GetByIdAsync(id);

        try
        {
            await _repository.DeleteAsync(id);
        }
        catch (Exception)
        {
            throw new OperationFailedException($"Failed to delete an order with id={id}.");
        }
    }
}
