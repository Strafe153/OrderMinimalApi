using Application.Services.Interfaces;
using Moq;

namespace MinimalApi.Tests.Fixtures;

public class OrderEndpointsFixture
{
	public Mock<IOrdersService> OrdersService { get; } = new();
}
