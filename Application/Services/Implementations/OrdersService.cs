using Application.Dtos.Order;
using Application.MappingRegistrations;
using Application.Services.Interfaces;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces.Repositories;
using Domain.Shared.Constants;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.Extensions.Logging;

namespace Application.Services.Implementations;

public class OrdersService : IOrdersService
{
	private readonly IOrdersRepository _repository;
	private readonly IOutputCacheStore _outputCacheStore;
	private readonly ILogger<OrdersService> _logger;

	public OrdersService(
		IOrdersRepository repository,
		IOutputCacheStore outputCacheStore,
		ILogger<OrdersService> logger)
	{
		_repository = repository;
		_outputCacheStore = outputCacheStore;
		_logger = logger;
	}

	public async Task<IEnumerable<OrderReadDto>> GetAllAsync(CancellationToken token)
	{
		var orders = await _repository.GetAllAsync(token);
		_logger.LogInformation("Successfully retrieved all the orders.");

		return orders.ToReadDto();
	}

	public async Task<OrderReadDto> GetByIdAsync(string id, CancellationToken token)
	{
		var order = await GetOrderByIdAsync(id, token);
		return order.ToReadDto();
	}

	public async Task<OrderReadDto> CreateAsync(OrderCreateDto dto, CancellationToken token)
	{
		var order = dto.ToOrder();
		await _repository.CreateAsync(order);

		await _outputCacheStore.EvictByTagAsync(CacheConstants.OrdersTag, token);
		_logger.LogInformation("Successfully created an order.");

		return order.ToReadDto();
	}

	public async Task UpdateAsync(string id, OrderUpdateDto newOrderDto, CancellationToken token)
	{
		var order = await GetOrderByIdAsync(id, token);
		newOrderDto.Update(order);

		try
		{
			await _repository.UpdateAsync(id, order);
		}
		catch
		{
			_logger.LogWarning("Failed to update the order with id='{Id}'.", id);
			throw new OperationFailedException($"Failed to update the order with id={id}.");
		}

		await _outputCacheStore.EvictByTagAsync(CacheConstants.OrdersTag, token);
		await _outputCacheStore.EvictByTagAsync(CacheConstants.OrderTag, token);

		_logger.LogInformation("Successfully updated the order with id='{Id}'", id);
	}

	public async Task DeleteAsync(string id, CancellationToken token)
	{
		await GetOrderByIdAsync(id, token);

		try
		{
			await _repository.DeleteAsync(id);
		}
		catch
		{
			_logger.LogWarning("Failed to update the order with id='{Id}'.", id);
			throw new OperationFailedException($"Failed to delete the order with id={id}.");
		}

		await _outputCacheStore.EvictByTagAsync(CacheConstants.OrdersTag, token);
		await _outputCacheStore.EvictByTagAsync(CacheConstants.OrderTag, token);

		_logger.LogInformation("Successfully deleted the order with id='{Id}'", id);
	}

	private async Task<Order> GetOrderByIdAsync(string id, CancellationToken token)
	{
		var order = await _repository.GetByIdAsync(id, token);

		if (order is null)
		{
			_logger.LogWarning("The order with id='{Id}' not found.", id);
			throw new NullReferenceException($"Order with id '{id}' not found.");
		}

		_logger.LogInformation("Successfully retrieved the order with id='{Id}'.", id);

		return order;
	}
}
