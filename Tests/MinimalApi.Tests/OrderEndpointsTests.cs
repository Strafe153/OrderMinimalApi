using Application.Dtos.Order;
using FakeItEasy;
using Microsoft.AspNetCore.Http;
using MinimalApi.Endpoints;
using MinimalApi.Tests.Fixtures;
using Shouldly;
using Xunit;

namespace Endpoints.Tests;

public class OrderEndpointsTests(OrderEndpointsFixture fixture) : IClassFixture<OrderEndpointsFixture>
{
	[Fact]
	public async Task GetAll_Should_ReturnOkOfOrderReadDto_WhenOrdersExist()
	{
		// Arrange
		A.CallTo(() => fixture.OrderService.GetAllAsync(A<CancellationToken>._))
			.Returns(fixture.OrderReadDtos);

		// Act
		var result = await OrderEndpoints.GetAll(fixture.OrderService, fixture.CancellationToken);
		var orders = result.Value;

		// Assert
		result.StatusCode.ShouldBe(StatusCodes.Status200OK);
		orders.ShouldNotBeNull();
		orders.Count().ShouldBe(fixture.OrderReadDtosCount);
	}

	[Fact]
	public async Task Get_Should_ReturnOkOfOrderReadDto_WhenOrderExists()
	{
		// Arrange
		A.CallTo(() => fixture.OrderService.GetByIdAsync(A<string>._, A<CancellationToken>._))
			.Returns(fixture.OrderReadDto);

		// Act
		var result = await OrderEndpoints.Get(fixture.OrderService, fixture.Id, fixture.CancellationToken);
		var order = result.Value;

		// Assert
		result.StatusCode.ShouldBe(StatusCodes.Status200OK);
		order.ShouldNotBeNull();
	}

	[Fact]
	public async Task Create_Should_ReturnCreatedOfOrderReadDto_WhenDataIsValid()
	{
		// Arrange
		A.CallTo(() => fixture.OrderService.CreateAsync(A<OrderCreateDto>._, A<CancellationToken>._))
			.Returns(fixture.OrderReadDto);

		// Act
		var result = await OrderEndpoints.Create(
			fixture.OrderService,
			fixture.OrderCreateDto,
			fixture.CancellationToken);

		var order = result.Value;

		// Assert
		result.StatusCode.ShouldBe(StatusCodes.Status201Created);
		order.ShouldNotBeNull();
	}

	[Fact]
	public async Task Update_Should_ReturnNoContent_WhenDataIsValid()
	{
		// Act
		var result = await OrderEndpoints.Update(
			fixture.OrderService,
			fixture.Id,
			fixture.OrderUpdateDto,
			fixture.CancellationToken);

		// Assert
		result.StatusCode.ShouldBe(StatusCodes.Status204NoContent);
	}

	[Fact]
	public async Task Delete_Should_ReturnNoContent_WhenDataIsValid()
	{
		// Act
		var result = await OrderEndpoints.Delete(fixture.OrderService, fixture.Id, fixture.CancellationToken);

		// Assert
		result.StatusCode.ShouldBe(StatusCodes.Status204NoContent);
	}
}
