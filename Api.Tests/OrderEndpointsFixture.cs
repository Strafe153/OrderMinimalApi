using Api.Features.Orders;
using Api.Features.Orders.CreateOrder;
using Api.Features.Orders.DeleteOrder;
using Api.Features.Orders.GetOrder;
using Api.Features.Orders.GetOrders;
using Api.Features.Orders.UpdateOrder;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.Extensions.Logging;
using Moq;

namespace Api.Tests;

public class OrderEndpointsFixture
{
	public Mock<IOrdersRepository> Repository { get; } = new();
	public Mock<IOutputCacheStore> OutputCache { get; } = new();
	public Mock<ILogger<GetOrdersEndpoint>> GetOrdersLogger { get; } = new();
	public Mock<ILogger<GetOrderEndpoint>> GetOrderLogger { get; } = new();
	public Mock<ILogger<CreateOrderEndpoint>> CreateOrderLogger { get; } = new();
	public Mock<ILogger<UpdateOrderEndpoint>> UpdateOrderLogger { get; } = new();
	public Mock<ILogger<DeleteOrderEndpoint>> DeleteOrderLogger { get; } = new();
}
