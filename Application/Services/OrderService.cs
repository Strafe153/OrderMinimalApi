using Domain.Dtos;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Mapster;
using Microsoft.Extensions.Logging;

namespace Application.Services;

public class OrderService : IOrderService
{
	private readonly IOrderRepository _repository;
	private readonly ILogger<OrderService> _logger;

	public OrderService(
		IOrderRepository repository,
		ILogger<OrderService> logger)
	{
		_repository = repository;
		_logger = logger;
	}

	public async Task<IEnumerable<OrderReadDto>> GetAllAsync(CancellationToken token = default)
	{
		var orders = await _repository.GetAllAsync(token);
		_logger.LogInformation("Successfully retrieved all the orders.");

		return orders.Adapt<IEnumerable<OrderReadDto>>();
	}

	public async Task<OrderReadDto> GetByIdAsync(string id, CancellationToken token = default)
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

	public async Task<OrderReadDto> CreateAsync(OrderCreateUpdateDto dto)
	{
		var order = dto.Adapt<Order>();
		await _repository.CreateAsync(order);

		_logger.LogInformation("Successfully created an order.");

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
			_logger.LogWarning("Failed to update the order with id='{Id}'.", id);
			throw new OperationFailedException($"Failed to update the order with id={id}.");
		}

		_logger.LogInformation("Successfully updated the order with id='{Id}'", id);
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
			_logger.LogWarning("Failed to update the order with id='{Id}'.", id);
			throw new OperationFailedException($"Failed to delete the order with id={id}.");
		}

		_logger.LogInformation("Successfully deleted the order with id='{Id}'", id);
	}
}
