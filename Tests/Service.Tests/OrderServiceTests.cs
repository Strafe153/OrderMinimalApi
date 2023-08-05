using FakeItEasy;
using FluentAssertions;
using MinimalApi.Dtos;
using MinimalApi.Entities;
using Service.Tests.Fixtures;
using Xunit;

namespace Service.Tests;

public class OrderServiceTests : IClassFixture<OrderServiceFixture>
{
    private readonly OrderServiceFixture _fixture;

    public OrderServiceTests(OrderServiceFixture fixture)
    {
        _fixture = fixture;   
    }

    [Fact]
    public async Task GetAllAsync_Should_ReturnIEnumerableOfOrderReadDtoFromRepository_WhenExists()
    {
        // Arrange
        A.CallTo(() => _fixture.CacheService.Get<IEnumerable<Order>>(A<string>._))
            .Returns(null!);

        A.CallTo(() => _fixture.OrderRepository.GetAllAsync(A<CancellationToken>._))
            .Returns(Task.FromResult(_fixture.Orders));

        // Act
        var result = await _fixture.OrderService.GetAllAsync();

        // Assert
        result.Count().Should().Be(_fixture.OrdersCount);
    }

    [Fact]
    public async Task GetAllAsync_Should_ReturnIEnumerableOfOrderReadDtoFromCache_WhenExists()
    {
        // Arrange
        A.CallTo(() => _fixture.CacheService.Get<IEnumerable<Order>>(A<string>._))
            .Returns(_fixture.Orders);

        // Act
        var result = await _fixture.OrderService.GetAllAsync();

        // Assert
        result.Count().Should().Be(_fixture.OrdersCount);
    }

    [Fact]
    public async Task GetByIdAsync_Should_ReturnOrderReadDtoFromRepository_WhenExists()
    {
        // Arrange
        A.CallTo(() => _fixture.CacheService.Get<Order>(A<string>._))
            .Returns(null!);

        A.CallTo(() => _fixture.OrderRepository.GetByIdAsync(A<string>._, A<CancellationToken>._))
            .Returns(Task.FromResult(_fixture.Order));

        // Act
        var result = await _fixture.OrderService.GetByIdAsync(_fixture.Id);

        // Assert
        result.Should().NotBeNull().And.BeOfType<OrderReadDto>();
    }

    [Fact]
    public async Task GetByIdAsync_Should_ReturnOrderReadDtoFromCache_WhenExists()
    {
        // Arrange
        A.CallTo(() => _fixture.CacheService.Get<Order>(A<string>._))
            .Returns(_fixture.Order);

        // Act
        var result = await _fixture.OrderService.GetByIdAsync(_fixture.Id);

        // Assert
        result.Should().NotBeNull().And.BeOfType<OrderReadDto>();
    }

    [Fact]
    public async Task GetByIdAsync_Should_ThrowNullReferenceException_WhenDoesNotExist()
    {
        // Arrange
        A.CallTo(() => _fixture.CacheService.Get<Order>(A<string>._))
            .Returns(null!);

        A.CallTo(() => _fixture.OrderRepository.GetByIdAsync(A<string>._, A<CancellationToken>._))
            .Returns(Task.FromResult((Order?)null));

        // Act
        var result = async () => await _fixture.OrderService.GetByIdAsync(_fixture.Id);

        // Assert
        await result.Should().ThrowAsync<NullReferenceException>();
    }

    [Fact]
    public async Task CreateAsync_Should_ReturnOrderReadDto_WhenDtoIsValid()
    {
        // Act
        var result = await _fixture.OrderService.CreateAsync(_fixture.OrderDto);

        // Assert
        result.Should().NotBeNull().And.BeOfType<OrderReadDto>();
    }

    [Fact]
    public async Task UpdateAsync_Should_ReturnTask_WhenDtoIsValid()
    {
        // Act
        var result = async () => await _fixture.OrderService.UpdateAsync(_fixture.Id, _fixture.OrderDto);

        // Assert
        await result.Should().NotThrowAsync<NullReferenceException>();
    }
}
