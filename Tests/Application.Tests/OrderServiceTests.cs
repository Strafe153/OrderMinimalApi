using Application.Dtos.Order;
using Application.Tests.Fixtures;
using Domain.Entities;
using MongoDB.Bson;
using Moq;
using Xunit;

namespace Application.Tests;

public class OrderServiceTests(OrderServiceFixture fixture) : IClassFixture<OrderServiceFixture>
{
	[Fact]
	public async Task GetAllAsync_Should_ReturnIEnumerableOfOrderReadDto_WhenOrdersExist()
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

		fixture.OrdersRepository
			.Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
			.ReturnsAsync(orders);

		// Act
		var sut = fixture.CreateSut();
		var result = await sut.GetAllAsync(new CancellationToken());

		// Assert
		Assert.Equal(2, result.Count());
		Assert.True(orders.All(o => !string.IsNullOrEmpty(o.Id)));
	}

	[Fact]
	public async Task GetByIdAsync_Should_ReturnOrderReadDto_WhenOrderExists()
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

		fixture.OrdersRepository
			.Setup(x => x.GetByIdAsync(id, It.IsAny<CancellationToken>()))
			.ReturnsAsync(order);

		// Act
		var sut = fixture.CreateSut();
		var result = await sut.GetByIdAsync(id, new CancellationToken());

		// Assert
		Assert.NotNull(result);
		Assert.Equal(id, order.Id);
		Assert.Equal(1399.99m, order.Price);
	}

	[Fact]
	public async Task GetByIdAsync_Should_ThrowNullReferenceException_WhenOrderDoesNotExist()
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

		fixture.OrdersRepository
			.Setup(x => x.GetByIdAsync(id, It.IsAny<CancellationToken>()))
			.ReturnsAsync(order);

		// Act
		var sut = fixture.CreateSut();
        Task<OrderReadDto> result() =>
			sut.GetByIdAsync(ObjectId.GenerateNewId().ToString(), new CancellationToken());

        // Assert
        await Assert.ThrowsAsync<NullReferenceException>(result);
	}

	[Fact]
	public async Task CreateAsync_Should_ReturnOrderReadDto_WhenDtoIsValid()
	{
		// Arrange
		OrderCreateDto createDto = new(
			"Robert Robertson",
			"SDN Torrance branch",
			"Mecha man suit",
			1399.99m
		);

		// Act
		var sut = fixture.CreateSut();
		var result = await sut.CreateAsync(createDto, new CancellationToken());

		// Assert
		Assert.NotNull(result);
		Assert.Equal("Robert Robertson", result.CustomerId);
		Assert.Equal("SDN Torrance branch", result.Address);
	}

	[Fact]
	public async Task UpdateAsync_Should_ReturnTask_WhenOrderExists()
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

		OrderUpdateDto updateDto = new(
			"Courtney Visi",
			"Torrance",
			"Inhaler",
			8.49m
		);

		fixture.OrdersRepository
			.Setup(x => x.GetByIdAsync(id, It.IsAny<CancellationToken>()))
			.ReturnsAsync(order);

		// Act
		var sut = fixture.CreateSut();

		var exception = await Record.ExceptionAsync(
			() => sut.UpdateAsync(id, updateDto, new CancellationToken()));

		// Assert
		Assert.Null(exception);
	}

	[Fact]
	public async Task UpdateAsync_Should_ThrowNullReferenceException_WhenOrderDoesNotExist()
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

		OrderUpdateDto updateDto = new(
			"Robert Robertson",
			"SDN Torrance branch",
			"Mecha man suit",
			1399.99m
		);

		fixture.OrdersRepository
			.Setup(x => x.GetByIdAsync(id, It.IsAny<CancellationToken>()))
			.ReturnsAsync(order);

		// Act
		var sut = fixture.CreateSut();

        Task result() => sut.UpdateAsync(
            ObjectId.GenerateNewId().ToString(),
            updateDto,
            new CancellationToken());

        // Assert
        await Assert.ThrowsAsync<NullReferenceException>(result);
	}

	[Fact]
	public async Task DeleteAsync_Should_ReturnTask_WhenOrderExists()
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

		fixture.OrdersRepository
			.Setup(x => x.GetByIdAsync(id, It.IsAny<CancellationToken>()))
			.ReturnsAsync(order);

		// Act
		var sut = fixture.CreateSut();
		var exception = await Record.ExceptionAsync(
			() => sut.DeleteAsync(id, new CancellationToken()));

		// Assert
		Assert.Null(exception);
	}

	[Fact]
	public async Task DeleteAsync_Should_ThrowNullReferenceException_WhenOrderDoesNotExist()
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

		fixture.OrdersRepository
			.Setup(x => x.GetByIdAsync(id, It.IsAny<CancellationToken>()))
			.ReturnsAsync(order);

		// Act
		var sut = fixture.CreateSut();

        Task result() => sut.DeleteAsync(
            ObjectId.GenerateNewId().ToString(),
            new CancellationToken());

        // Assert
        await Assert.ThrowsAsync<NullReferenceException>(result);
	}
}
