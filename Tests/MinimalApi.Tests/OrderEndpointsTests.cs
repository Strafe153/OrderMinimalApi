using Application.Dtos.Order;
using Microsoft.AspNetCore.Http;
using MinimalApi.Endpoints;
using MinimalApi.Tests.Fixtures;
using MongoDB.Bson;
using Moq;
using Xunit;

namespace Endpoints.Tests;

public class OrderEndpointsTests(OrderEndpointsFixture fixture) : IClassFixture<OrderEndpointsFixture>
{
	[Fact]
	public async Task GetAll_Should_ReturnOkOfOrderReadDto_WhenOrdersExist()
	{
		// Arrange
		IEnumerable<OrderReadDto> readDtos = [
			new(
				ObjectId.GenerateNewId().ToString(),
				"Robert Robertson",
				"SDN Torrance branch",
				"Mecha man suit",
				1399.99m),
			new(
				ObjectId.GenerateNewId().ToString(),
				"Courtney Visi",
				"Torrance",
				"Inhaler",
				8.49m)
		];

		fixture.OrdersService
			.Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
			.ReturnsAsync(readDtos);

		// Act
		var result = await OrderEndpoints.GetAll(
			fixture.OrdersService.Object,
			new CancellationToken());

		var orders = result.Value;

		// Assert
		Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
		Assert.NotNull(orders);
		Assert.Equal(2, orders.Count());
		Assert.True(orders.All(o => !string.IsNullOrEmpty(o.Id)));
	}

	[Fact]
	public async Task Get_Should_ReturnOkOfOrderReadDto_WhenOrderExists()
	{
		// Arrange
		var id = ObjectId.GenerateNewId().ToString();

		OrderReadDto readDto = new(
			id,
			"Robert Robertson",
			"SDN Torrance branch",
			"Mecha man suit",
			1399.99m);

		fixture.OrdersService
			.Setup(x => x.GetByIdAsync(id, It.IsAny<CancellationToken>()))
			.ReturnsAsync(readDto);

		// Act
		var result = await OrderEndpoints.Get(
			fixture.OrdersService.Object,
			id,
			new CancellationToken());

		var order = result.Value;

		// Assert
		Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
		Assert.NotNull(order);
		Assert.Equal(id, order.Id);
	}

	[Fact]
	public async Task Create_Should_ReturnCreatedOfOrderReadDto_WhenDataIsValid()
	{
		// Arrange
		OrderCreateDto createDto = new(
			"Robert Robertson",
			"SDN Torrance branch",
			"Mecha man suit",
			1399.99m
		);

		OrderReadDto readDto = new(
			ObjectId.GenerateNewId().ToString(),
			"Robert Robertson",
			"SDN Torrance branch",
			"Mecha man suit",
			1399.99m);

		fixture.OrdersService
			.Setup(x => x.CreateAsync(createDto, new CancellationToken()))
			.ReturnsAsync(readDto);

		// Act
		var result = await OrderEndpoints.Create(
			fixture.OrdersService.Object,
			createDto,
			new CancellationToken());

		var order = result.Value;

		// Assert
		Assert.Equal(StatusCodes.Status201Created, result.StatusCode);
		Assert.NotNull(order);
		Assert.Equal("Mecha man suit", order.Product);
	}

	[Fact]
	public async Task Update_Should_ReturnNoContent_WhenDataIsValid()
	{
		// Act
		var id = ObjectId.GenerateNewId().ToString();

		OrderReadDto readDto = new(
			id,
			"Robert Robertson",
			"SDN Torrance branch",
			"Mecha man suit",
			1399.99m);

		OrderUpdateDto updateDto = new(
			"Courtney Visi",
			"Torrance",
			"Inhaler",
			8.49m
		);

		fixture.OrdersService
			.Setup(x => x.GetByIdAsync(id, It.IsAny<CancellationToken>()))
			.ReturnsAsync(readDto);

		var result = await OrderEndpoints.Update(
			fixture.OrdersService.Object,
			id,
			updateDto,
			new CancellationToken());

		// Assert
		Assert.Equal(StatusCodes.Status204NoContent, result.StatusCode);
	}

	[Fact]
	public async Task Delete_Should_ReturnNoContent_WhenDataIsValid()
	{
		// Arrange
		var id = ObjectId.GenerateNewId().ToString();

		OrderReadDto readDto = new(
			id,
			"Robert Robertson",
			"SDN Torrance branch",
			"Mecha man suit",
			1399.99m);

		fixture.OrdersService
			.Setup(x => x.GetByIdAsync(id, It.IsAny<CancellationToken>()))
			.ReturnsAsync(readDto);

		// Act
		var result = await OrderEndpoints.Delete(fixture.OrdersService.Object, id, new CancellationToken());

		// Assert
		Assert.Equal(StatusCodes.Status204NoContent, result.StatusCode);
	}
}
