﻿using Application.Dtos.Order;
using FakeItEasy;
using Microsoft.AspNetCore.Http;
using MinimalApi.Endpoints;
using MinimalApi.Tests.Fixtures;
using Shouldly;
using Xunit;

namespace Endpoints.Tests;

public class OrderEndpointsTests : IClassFixture<OrderEndpointsFixture>
{
	private readonly OrderEndpointsFixture _fixture;

	public OrderEndpointsTests(OrderEndpointsFixture fixture)
	{
		_fixture = fixture;
	}

	[Fact]
	public async Task GetAll_Should_ReturnOkOfOrderReadDto_WhenOrdersExist()
	{
		// Arrange
		A.CallTo(() => _fixture.OrderService.GetAllAsync(A<CancellationToken>._))
			.Returns(_fixture.OrderReadDtos);

		// Act
		var result = await OrderEndpoints.GetAll(_fixture.OrderService, _fixture.CancellationToken);
		var orders = result.Value;

		// Assert
		result.StatusCode.ShouldBe(StatusCodes.Status200OK);
		orders.ShouldNotBeNull();
		orders.Count().ShouldBe(_fixture.OrderReadDtosCount);
	}

	[Fact]
	public async Task Get_Should_ReturnOkOfOrderReadDto_WhenOrderExists()
	{
		// Arrange
		A.CallTo(() => _fixture.OrderService.GetByIdAsync(A<string>._, A<CancellationToken>._))
			.Returns(_fixture.OrderReadDto);

		// Act
		var result = await OrderEndpoints.Get(_fixture.OrderService, _fixture.Id, _fixture.CancellationToken);
		var order = result.Value;

		// Assert
		result.StatusCode.ShouldBe(StatusCodes.Status200OK);
		order.ShouldNotBeNull();
	}

	[Fact]
	public async Task Create_Should_ReturnCreatedOfOrderReadDto_WhenDataIsValid()
	{
		// Arrange
		A.CallTo(() => _fixture.OrderService.CreateAsync(A<OrderCreateDto>._, A<CancellationToken>._))
			.Returns(_fixture.OrderReadDto);

		// Act
		var result = await OrderEndpoints.Create(
			_fixture.OrderService,
			_fixture.OrderCreateDto,
			_fixture.CancellationToken);

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
			_fixture.OrderService,
			_fixture.Id,
			_fixture.OrderUpdateDto,
			_fixture.CancellationToken);

		// Assert
		result.StatusCode.ShouldBe(StatusCodes.Status204NoContent);
	}

	[Fact]
	public async Task Delete_Should_ReturnNoContent_WhenDataIsValid()
	{
		// Act
		var result = await OrderEndpoints.Delete(_fixture.OrderService, _fixture.Id, _fixture.CancellationToken);

		// Assert
		result.StatusCode.ShouldBe(StatusCodes.Status204NoContent);
	}
}
