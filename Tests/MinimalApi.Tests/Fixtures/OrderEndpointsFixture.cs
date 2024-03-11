using AutoFixture;
using AutoFixture.AutoFakeItEasy;
using Bogus;
using Domain.Dtos;
using Domain.Interfaces.Services;
using MongoDB.Bson;

namespace MinimalApi.Tests.Fixtures;

public class OrderEndpointsFixture
{
	public OrderEndpointsFixture()
	{
		var fixture = new Fixture().Customize(new AutoFakeItEasyCustomization());

		Id = ObjectId.GenerateNewId().ToString();
		OrderReadDtosCount = Random.Shared.Next(1, 21);

		var orderReadDtoFaker = new Faker<OrderReadDto>()
			.CustomInstantiator(f => new(
				Id,
				f.Name.FullName(),
				f.Address.FullAddress(),
				f.Commerce.Product(),
				f.Random.Decimal()));

		var orderCreateUpdateDtoFaker = new Faker<OrderCreateUpdateDto>()
			.CustomInstantiator(f => new(
				f.Name.FullName(),
				f.Address.FullAddress(),
				f.Commerce.Product(),
				f.Random.Decimal()));

		OrderService = fixture.Freeze<IOrderService>();

		OrderReadDto = orderReadDtoFaker.Generate();
		OrderCreateUpdateDto = orderCreateUpdateDtoFaker.Generate();
		OrderReadDtos = orderReadDtoFaker.Generate(OrderReadDtosCount);
	}

	public IOrderService OrderService { get; }
	public CancellationToken CancellationToken { get; }

	public string Id { get; }
	public int OrderReadDtosCount { get; }
	public OrderReadDto OrderReadDto { get; }
	public OrderCreateUpdateDto OrderCreateUpdateDto { get; }
	public IEnumerable<OrderReadDto> OrderReadDtos { get; }
}
