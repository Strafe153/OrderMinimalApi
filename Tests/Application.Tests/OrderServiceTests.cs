using Application.Dtos;
using Application.Tests.Fixtures;
using Domain.Entities;
using Domain.Exceptions;
using FakeItEasy;
using Shouldly;
using Xunit;

namespace Application.Tests;

public class OrderServiceTests : IClassFixture<OrderServiceFixture>
{
	private readonly OrderServiceFixture _fixture;

	public OrderServiceTests(OrderServiceFixture fixture)
	{
		_fixture = fixture;
	}

	[Fact]
	public async Task GetAllAsync_Should_ReturnIEnumerableOfOrderReadDto_WhenOrdersExist()
	{
		// Arrange
		A.CallTo(() => _fixture.OrderRepository.GetAllAsync(A<CancellationToken>._))
			.Returns(Task.FromResult(_fixture.Orders));

		// Act
		var result = await _fixture.OrderService.GetAllAsync();

		// Assert
		result.Count().ShouldBe(_fixture.OrdersCount);
	}

	[Fact]
	public async Task GetByIdAsync_Should_ReturnOrderReadDto_WhenOrderExists()
	{
		// Arrange
		A.CallTo(() => _fixture.OrderRepository.GetByIdAsync(A<string>._, A<CancellationToken>._))
			.Returns(Task.FromResult(_fixture.Order));

		// Act
		var result = await _fixture.OrderService.GetByIdAsync(_fixture.Id);

		// Assert
		result.ShouldNotBeNull();
		result.ShouldBeOfType<OrderReadDto>();
	}

	[Fact]
	public async Task GetByIdAsync_Should_ThrowNullReferenceException_WhenOrderDoesNotExist()
	{
		// Arrange
		A.CallTo(() => _fixture.OrderRepository.GetByIdAsync(A<string>._, A<CancellationToken>._))
			.Returns(Task.FromResult((Order?)null));

		// Act
		var result = async () => await _fixture.OrderService.GetByIdAsync(_fixture.Id);

		// Assert
		await result.ShouldThrowAsync<NullReferenceException>();
	}

	[Fact]
	public async Task CreateAsync_Should_ReturnOrderReadDto_WhenDtoIsValid()
	{
		// Act
		var result = await _fixture.OrderService.CreateAsync(_fixture.OrderDto);

		// Assert
		result.ShouldNotBeNull();
		result.ShouldBeOfType<OrderReadDto>();
	}

	[Fact]
	public async Task UpdateAsync_Should_ReturnTask_WhenOrderExists()
	{
		// Arrange
		A.CallTo(() => _fixture.OrderRepository.UpdateAsync(A<string>._, A<Order>._))
			.Returns(Task.CompletedTask);

		// Act
		var result = async () => await _fixture.OrderService.UpdateAsync(_fixture.Id, _fixture.OrderDto);

		// Assert
		await result.ShouldNotThrowAsync();
	}

	[Fact]
	public async Task UpdateAsync_Should_ThrowNullReferenceException_WhenOrderDoesNotExist()
	{
		// Arrange
		A.CallTo(() => _fixture.OrderRepository.GetByIdAsync(A<string>._, A<CancellationToken>._))
			.Returns(Task.FromResult((Order?)null));

		// Act
		var result = async () => await _fixture.OrderService.UpdateAsync(_fixture.Id, _fixture.OrderDto);

		// Assert
		await result.ShouldThrowAsync<NullReferenceException>();
	}

	[Fact]
	public async Task UpdateAsync_Should_ThrowOperationFailedException_WhenUpdateFails()
	{
		// Arrange
		A.CallTo(() => _fixture.OrderRepository.UpdateAsync(A<string>._, A<Order>._))
			.ThrowsAsync(_fixture.OperationFailedException);

		// Act
		var result = async () => await _fixture.OrderService.UpdateAsync(_fixture.Id, _fixture.OrderDto);

		// Assert
		await result.ShouldThrowAsync<OperationFailedException>();
	}

	[Fact]
	public async Task DeleteAsync_Should_ReturnTask_WhenOrderExists()
	{
		// Arrange
		A.CallTo(() => _fixture.OrderRepository.GetByIdAsync(A<string>._, A<CancellationToken>._))
			.Returns(Task.FromResult(_fixture.Order));

		// Act
		var result = async () => await _fixture.OrderService.DeleteAsync(_fixture.Id);

		// Assert
		await result.ShouldNotThrowAsync();
	}

	[Fact]
	public async Task DeleteAsync_Should_ThrowNullReferenceException_WhenOrderDoesNotExist()
	{
		// Arrange
		A.CallTo(() => _fixture.OrderRepository.GetByIdAsync(A<string>._, A<CancellationToken>._))
			.Returns(Task.FromResult((Order?)null));

		// Act
		var result = async () => await _fixture.OrderService.DeleteAsync(_fixture.Id);

		// Assert
		await result.ShouldThrowAsync<NullReferenceException>();
	}

	[Fact]
	public async Task DeleteAsync_Should_ThrowOperationFailedException_WhenDeleteFails()
	{
		// Arrange
		A.CallTo(() => _fixture.OrderRepository.DeleteAsync(A<string>._))
			.ThrowsAsync(_fixture.OperationFailedException);

		// Act
		var result = async () => await _fixture.OrderService.DeleteAsync(_fixture.Id);

		// Assert
		await result.ShouldThrowAsync<OperationFailedException>();
	}
}
