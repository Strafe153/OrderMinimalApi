using Application.Dtos.Order;
using Application.Services.Implementations;
using AutoFixture;
using AutoFixture.AutoFakeItEasy;
using Bogus;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces.Repositories;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;

namespace Application.Tests.Fixtures;

public class OrderServiceFixture
{
	public OrderServiceFixture()
	{
		var fixture = new Fixture().Customize(new AutoFakeItEasyCustomization());

		Id = ObjectId.GenerateNewId().ToString();

		var orderFaker = new Faker<Order>()
			.RuleFor(o => o.Id, Id)
			.RuleFor(o => o.CustomerName, f => f.Random.String2(16))
			.RuleFor(o => o.Address, f => f.Address.FullAddress())
			.RuleFor(o => o.Product, f => f.Commerce.Product())
			.RuleFor(o => o.Price, f => f.Random.Decimal());

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

		var operationFailedExceptionFaker = new Faker<OperationFailedException>()
			.CustomInstantiator(f => new(f.Lorem.Sentence()));

		OrderRepository = fixture.Freeze<IOrdersRepository>();
		OutputCacheStore = fixture.Freeze<IOutputCacheStore>();
		Logger = fixture.Freeze<ILogger<OrdersService>>();

		OrderService = new OrdersService(OrderRepository, OutputCacheStore, Logger);

		OrdersCount = Random.Shared.Next(2, 21);
		OperationFailedException = operationFailedExceptionFaker.Generate();
		Order = orderFaker.Generate();
		OrderCreateDto = orderCreateDtoFaker.Generate();
		OrderUpdateDto = orderUpdateDtoFaker.Generate();
		Orders = orderFaker.Generate(OrdersCount);
	}

	public OrdersService OrderService { get; }
	public IOrdersRepository OrderRepository { get; }
	public IOutputCacheStore OutputCacheStore { get; set; }
	public ILogger<OrdersService> Logger { get; }
	public CancellationToken CancellationToken { get; set; }

	public string Id { get; }
	public int OrdersCount { get; }
	public OperationFailedException OperationFailedException { get; }
	public Order? Order { get; }
	public OrderCreateDto OrderCreateDto { get; }
	public OrderUpdateDto OrderUpdateDto { get; }
	public IEnumerable<Order> Orders { get; }
}
