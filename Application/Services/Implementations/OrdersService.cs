using Application.Dtos.Order;
using Application.Services.Interfaces;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces.Repositories;
using Mapster;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.OutputCaching;
using Domain.Shared.Constants;

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

		return orders.Adapt<IEnumerable<OrderReadDto>>();
	}

	public async Task<OrderReadDto> GetByIdAsync(string id, CancellationToken token)
	{
		var order = await _repository.GetByIdAsync(id, token);

		if (order is null)
		{
			_logger.LogWarning("The order with id='{Id}' not found.", id);
			throw new NullReferenceException($"Order with id '{id}' not found.");
		}

		_logger.LogInformation("Successfully retrieved the order with id='{Id}'.", id);

		return order.Adapt<OrderReadDto>();
	}

	public async Task<OrderReadDto> CreateAsync(OrderCreateDto dto, CancellationToken token)
	{
		var order = dto.Adapt<Order>();
		await _repository.CreateAsync(order);

		await _outputCacheStore.EvictByTagAsync(CacheConstants.OrdersTag, token);
		_logger.LogInformation("Successfully created an order.");

		return order.Adapt<OrderReadDto>();
	}

	public async Task UpdateAsync(string id, OrderUpdateDto newOrderDto, CancellationToken token)
	{
		var readDto = await GetByIdAsync(id, token);

		try
		{
			newOrderDto.Adapt(readDto);

			var order = readDto.Adapt<Order>();
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
		await GetByIdAsync(id, token);

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
}
