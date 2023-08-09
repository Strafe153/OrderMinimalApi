using Core.Dtos;
using Core.Entities;
using Core.Exceptions;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Mapster;

namespace Core.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _repository;

    public OrderService(IOrderRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<OrderReadDto>> GetAllAsync(CancellationToken token = default)
    {
        var orders = await _repository.GetAllAsync(token);
        return orders.Adapt<IEnumerable<OrderReadDto>>();
    }

    public async Task<OrderReadDto> GetByIdAsync(string id, CancellationToken token = default)
    {
        var order = await _repository.GetByIdAsync(id, token);

        if (order is null)
        {
            throw new NullReferenceException($"Order with id '{id}' not found.");
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
