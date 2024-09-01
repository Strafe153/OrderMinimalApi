using Application.Dtos.Order;
using Application.Services.Interfaces;
using AutoFixture;
using AutoFixture.AutoFakeItEasy;
using Bogus;
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

		var orderCreateDtoFaker = new Faker<OrderCreateDto>()
			.CustomInstantiator(f => new(
				f.Name.FullName(),
				f.Address.FullAddress(),
				f.Commerce.Product(),
				f.Random.Decimal()));

		var orderUpdateDtoFaker = new Faker<OrderUpdateDto>()
			.CustomInstantiator(f => new(
				f.Name.FullName(),
				f.Address.FullAddress(),
				f.Commerce.Product(),
				f.Random.Decimal()));

		OrderService = fixture.Freeze<IOrdersService>();

		OrderReadDto = orderReadDtoFaker.Generate();
		OrderCreateDto = orderCreateDtoFaker.Generate();
		OrderUpdateDto = orderUpdateDtoFaker.Generate();
		OrderReadDtos = orderReadDtoFaker.Generate(OrderReadDtosCount);
	}

	public IOrdersService OrderService { get; }
	public CancellationToken CancellationToken { get; }

	public string Id { get; }
	public int OrderReadDtosCount { get; }
	public OrderReadDto OrderReadDto { get; }
	public OrderCreateDto OrderCreateDto { get; }
	public OrderUpdateDto OrderUpdateDto { get; }
	public IEnumerable<OrderReadDto> OrderReadDtos { get; }
}
