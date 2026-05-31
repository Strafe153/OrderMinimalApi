using Application.Services.Implementations;
using Domain.Interfaces.Repositories;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.Extensions.Logging;
using Moq;

namespace Application.Tests.Fixtures;

public class OrderServiceFixture
{
	public Mock<IOrdersRepository> OrdersRepository { get; } = new();
	public Mock<IOutputCacheStore> OutputCacheStore { get; } = new();
	public Mock<ILogger<OrdersService>> Logger { get; } = new();

	public OrdersService CreateSut()
	{
		OrdersService service = new(
			OrdersRepository.Object,
			OutputCacheStore.Object,
			Logger.Object
		);

		return service;
	}
}
