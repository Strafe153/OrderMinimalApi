using Core.Dtos;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using MinimalApi.Endpoints;
using MinimalApi.Tests.Fixtures;
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
    public async Task GetAllAsync_Should_ReturnOkOfOrderReadDto_WhenOrdersExist()
    {
        // Arrange
        A.CallTo(() => _fixture.OrderService.GetAllAsync(A<CancellationToken>._))
            .Returns(_fixture.OrderReadDtos);

        // Act
        var result = await OrderEndpoints.GetAllAsync(_fixture.OrderService, _fixture.CancellationToken);
        var orders = result.Value.As<IEnumerable<OrderReadDto>>();

        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status200OK);
        orders.Count().Should().Be(_fixture.OrderReadDtosCount);
    }

    [Fact]
    public async Task GetAsync_Should_ReturnOkOfOrderReadDto_WhenOrderExists()
    {
        // Arrange
        A.CallTo(() => _fixture.OrderService.GetByIdAsync(A<string>._, A<CancellationToken>._))
            .Returns(_fixture.OrderReadDto);

        // Act
        var result = await OrderEndpoints.GetAsync(_fixture.OrderService, _fixture.Id, _fixture.CancellationToken);
        var order = result.Value;

        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status200OK);
        order.Should().NotBeNull();
    }

    [Fact]
    public async Task CreateAsync_Should_ReturnCreatedOfOrderReadDto_WhenDataIsValid()
    {
        // Arrange
        A.CallTo(() => _fixture.OrderService.CreateAsync(A<OrderCreateUpdateDto>._))
            .Returns(_fixture.OrderReadDto);

        // Act
        var result = await OrderEndpoints.CreateAsync(_fixture.OrderService, _fixture.OrderCreateUpdateDto);
        var order = result.Value;

        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status201Created);
        order.Should().NotBeNull();
    }

    [Fact]
    public async Task UpdateAsync_Should_ReturnNoContent_WhenDataIsValid()
    {
        // Act
        var result = await OrderEndpoints.UpdateAsync(_fixture.OrderService, _fixture.Id, _fixture.OrderCreateUpdateDto);

        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status204NoContent);
    }

    [Fact]
    public async Task DeleteAsync_Should_ReturnNoContent_WhenDataIsValid()
    {
        // Act
        var result = await OrderEndpoints.DeleteAsync(_fixture.OrderService, _fixture.Id);

        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status204NoContent);
    }
}
