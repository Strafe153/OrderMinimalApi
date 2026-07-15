using Api.Entities;
using Api.Features.Orders.CreateOrder;
using Api.Features.Orders.DeleteOrder;
using Api.Features.Orders.GetOrder;
using Api.Features.Orders.GetOrders;
using Api.Features.Orders.UpdateOrder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using MongoDB.Bson;
using Moq;
using Xunit;

namespace Api.Tests;

public class OrderEndpointsTests(OrderEndpointsFixture fixture) : IClassFixture<OrderEndpointsFixture>
{
	[Fact]
	public async Task GetOrders_Should_ReturnOkOfGetOrdersResponse_WhenOrdersExist()
	{
		// Arrange
		IEnumerable<Order> orders = [
			new()
			{
				Id = ObjectId.GenerateNewId().ToString(),
				CustomerName = "Robert Robertson",
				Address = "SDN Torrance branch",
				Product = "Mecha man suit",
				Price = 1399.99m
			},
			new()
			{
				Id = ObjectId.GenerateNewId().ToString(),
				CustomerName = "Courtney Visi",
				Address = "Torrance",
				Product = "Inhaler",
				Price = 8.49m
			},
		];

		fixture.Repository
			.Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
			.ReturnsAsync(orders);

		// Act
		var result = await GetOrdersEndpoint.Handle(
			fixture.Repository.Object,
			fixture.GetOrdersLogger.Object,
			new CancellationToken());

		var orderResponse = result.Value;

		// Assert
		Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
		Assert.NotNull(orderResponse);
		Assert.Equal(2, orderResponse.Count);
		Assert.True(orderResponse.Items.All(o => !string.IsNullOrEmpty(o.Id)));
	}

	[Fact]
	public async Task GetOrder_Should_ReturnOkOfGetOrderResponse_WhenOrderExists()
	{
		// Arrange
		var id = ObjectId.GenerateNewId().ToString();

		Order order = new()
		{
			Id = id,
			CustomerName = "Robert Robertson",
			Address = "SDN Torrance branch",
			Product = "Mecha man suit",
			Price = 1399.99m
		};

		fixture.Repository
			.Setup(x => x.GetByIdAsync(id, It.IsAny<CancellationToken>()))
			.ReturnsAsync(order);

		// Act
		var result = await GetOrderEndpoint.Handle(
			id,
			fixture.Repository.Object,
			fixture.GetOrderLogger.Object,
			new CancellationToken());

		var response = result.Value;

		// Assert
		Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
		Assert.NotNull(order);
		Assert.Equal(id, order.Id);
	}

	[Fact]
	public async Task GetOrder_Should_ThrowNullReferenceException_WhenOrderDoesNotExist()
	{
		// Arrange
		var id = ObjectId.GenerateNewId().ToString();

		Order order = new()
		{
			Id = id,
			CustomerName = "Robert Robertson",
			Address = "SDN Torrance branch",
			Product = "Mecha man suit",
			Price = 1399.99m
		};

		fixture.Repository
			.Setup(x => x.GetByIdAsync(id, It.IsAny<CancellationToken>()))
			.ReturnsAsync(order);

        // Act
        Task<Ok<GetOrderResponse>> result() => GetOrderEndpoint.Handle(
            ObjectId.GenerateNewId().ToString(),
            fixture.Repository.Object,
			fixture.GetOrderLogger.Object,
            new CancellationToken());

        // Assert
        await Assert.ThrowsAsync<NullReferenceException>(result);
	}

	[Fact]
	public async Task CreateOrder_Should_ReturnCreatedOfCreateOrderResponse_WhenDataIsValid()
	{
		// Arrange
		CreateOrderRequest request = new(
			"Robert Robertson",
			"SDN Torrance branch",
			"Mecha man suit",
			1399.99m);

		// Act
		var result = await CreateOrderEndpoint.Handle(
			request,
			fixture.Repository.Object,
			fixture.OutputCache.Object,
			fixture.CreateOrderLogger.Object,
			new CancellationToken());

		var order = result.Value;

		// Assert
		Assert.Equal(StatusCodes.Status201Created, result.StatusCode);
		Assert.NotNull(order);
		Assert.Equal("Mecha man suit", order.Product);
	}

	// [Fact]
	// public async Task CreateOrder_Should_ThrowOperationFailedException_WhenDataIsInvalid()
	// {
	// 	// Arrange
	// 	CreateOrderRequest request = new(
	// 		"Robert Robertson",
	// 		"SDN Torrance branch",
	// 		"Mecha man suit",
	// 		1399.99m);

	// 	// Act
	// 	var result = await CreateOrderEndpoint.Handle(
	// 		request,
	// 		fixture.Repository.Object,
	// 		fixture.OutputCache.Object,
	// 		fixture.CreateOrderLogger.Object,
	// 		new CancellationToken());

	// 	var order = result.Value;

	// 	// Assert
	// 	Assert.Equal(StatusCodes.Status201Created, result.StatusCode);
	// 	Assert.NotNull(order);
	// 	Assert.Equal("Mecha man suit", order.Product);
	// }

	[Fact]
	public async Task UpdateOrder_Should_ReturnNoContent_WhenOrderExists()
	{
		// Act
		var id = ObjectId.GenerateNewId().ToString();

		Order order = new()
		{
			Id = id,
			CustomerName = "Robert Robertson",
			Address = "SDN Torrance branch",
			Product = "Mecha man suit",
			Price = 1399.99m
		};

		UpdateOrderRequest request = new(
			"Courtney Visi",
			"Torrance",
			"Inhaler",
			8.49m
		);

		fixture.Repository
			.Setup(x => x.GetByIdAsync(id, It.IsAny<CancellationToken>()))
			.ReturnsAsync(order);

		var result = await UpdateOrderEndpoint.Handle(
			id,
			request,
			fixture.Repository.Object,
			fixture.OutputCache.Object,
			fixture.UpdateOrderLogger.Object,
			new CancellationToken());

		// Assert
		Assert.Equal(StatusCodes.Status204NoContent, result.StatusCode);
	}

	[Fact]
	public async Task UpdateOrder_Should_ThrowNullReferenceException_WhenOrderDoesNotExist()
	{
		// Act
		var id = ObjectId.GenerateNewId().ToString();

		Order order = new()
		{
			Id = id,
			CustomerName = "Robert Robertson",
			Address = "SDN Torrance branch",
			Product = "Mecha man suit",
			Price = 1399.99m
		};

		UpdateOrderRequest request = new(
			"Courtney Visi",
			"Torrance",
			"Inhaler",
			8.49m
		);

		fixture.Repository
			.Setup(x => x.GetByIdAsync(id, It.IsAny<CancellationToken>()))
			.ReturnsAsync(order);

		Task<NoContent> result() => UpdateOrderEndpoint.Handle(
			ObjectId.GenerateNewId().ToString(),
			request,
			fixture.Repository.Object,
			fixture.OutputCache.Object,
			fixture.UpdateOrderLogger.Object,
			new CancellationToken());

		// Assert
        await Assert.ThrowsAsync<NullReferenceException>(result);
	}

	[Fact]
	public async Task DeleteOrder_Should_ReturnNoContent_WhenOrderExists()
	{
		// Arrange
		var id = ObjectId.GenerateNewId().ToString();

		Order order = new()
		{
			Id = id,
			CustomerName = "Robert Robertson",
			Address = "SDN Torrance branch",
			Product = "Mecha man suit",
			Price = 1399.99m
		};

		fixture.Repository
			.Setup(x => x.GetByIdAsync(id, It.IsAny<CancellationToken>()))
			.ReturnsAsync(order);

		// Act
		var result = await DeleteOrderEndpoint.Handle(
			id,
			fixture.Repository.Object,
			fixture.OutputCache.Object,
			fixture.DeleteOrderLogger.Object,
			new CancellationToken());

		// Assert
		Assert.Equal(StatusCodes.Status204NoContent, result.StatusCode);
	}

	[Fact]
	public async Task DeleteOrder_Should_ThrowNullReferenceException_WhenOrderDoesNotExist()
	{
		// Arrange
		var id = ObjectId.GenerateNewId().ToString();

		Order order = new()
		{
			Id = id,
			CustomerName = "Robert Robertson",
			Address = "SDN Torrance branch",
			Product = "Mecha man suit",
			Price = 1399.99m
		};

		fixture.Repository
			.Setup(x => x.GetByIdAsync(id, It.IsAny<CancellationToken>()))
			.ReturnsAsync(order);

		// Act
		Task<NoContent> result() => DeleteOrderEndpoint.Handle(
			ObjectId.GenerateNewId().ToString(),
			fixture.Repository.Object,
			fixture.OutputCache.Object,
			fixture.DeleteOrderLogger.Object,
			new CancellationToken());

		// Assert
        await Assert.ThrowsAsync<NullReferenceException>(result);
	}
}
