using AutoFixture;
using AutoFixture.AutoFakeItEasy;
using Bogus;
using MinimalApi.Dtos;
using MinimalApi.Entities;
using MinimalApi.Exceptions;
using MinimalApi.Repositories.Abstractions;
using MinimalApi.Services;
using MinimalApi.Services.Abstractions;
using MongoDB.Bson;

namespace Services.Tests.Fixtures;

public class OrderServiceFixture
{
    public OrderServiceFixture()
    {
        var fixture = new Fixture().Customize(new AutoFakeItEasyCustomization());

        Id = ObjectId.GenerateNewId().ToString();

        var orderFaker = new Faker<Order>()
            .RuleFor(o => o.Id, Id)
            .RuleFor(o => o.CustomerName, f => f.Name.FullName())
            .RuleFor(o => o.Address, f => f.Address.FullAddress())
            .RuleFor(o => o.Product, f => f.Commerce.Product())
            .RuleFor(o => o.Price, f => f.Random.Decimal());

        var orderDtoFaker = new Faker<OrderCreateUpdateDto>()
            .RuleFor(o => o.CustomerName, f => f.Name.FullName())
            .RuleFor(o => o.Address, f => f.Address.FullAddress())
            .RuleFor(o => o.Product, f => f.Commerce.Product())
            .RuleFor(o => o.Price, f => f.Random.Decimal());

        var operationFailedExceptionFaker = new Faker<OperationFailedException>()
            .CustomInstantiator(f => new(f.Lorem.Sentence()));

        OrderRepository = fixture.Freeze<IOrderRepository>();
        CacheService = fixture.Freeze<ICacheService>();

        OrderService = new OrderService(OrderRepository, CacheService);

        OrdersCount = Random.Shared.Next(2, 21);
        OperationFailedException = operationFailedExceptionFaker.Generate();
        Order = orderFaker.Generate();
        OrderDto = orderDtoFaker.Generate();
        Orders = orderFaker.Generate(OrdersCount);
    }

    public OrderService OrderService { get; }
    public IOrderRepository OrderRepository { get; }
    public ICacheService CacheService { get; }

    public string Id { get; }
    public int OrdersCount { get; }
    public OperationFailedException OperationFailedException { get; }
    public Order? Order { get; }
    public OrderCreateUpdateDto OrderDto { get; }
    public IEnumerable<Order> Orders { get; }
}
