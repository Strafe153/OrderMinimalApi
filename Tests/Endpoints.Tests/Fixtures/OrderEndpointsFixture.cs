using AutoFixture;
using AutoFixture.AutoFakeItEasy;
using Bogus;
using MinimalApi.Dtos;
using MinimalApi.Services.Abstractions;
using MongoDB.Bson;

namespace Endpoints.Tests.Fixtures;

public class OrderEndpointsFixture
{
    public OrderEndpointsFixture()
    {
        var fixture = new Fixture().Customize(new AutoFakeItEasyCustomization());

        Id = ObjectId.GenerateNewId().ToString();
        OrderReadDtosCount = Random.Shared.Next(1, 21);

        var orderReadDtoFaker = new Faker<OrderReadDto>()
            .RuleFor(o => o.Id, Id)
            .RuleFor(o => o.CustomerName, f => f.Name.FullName())
            .RuleFor(o => o.Address, f => f.Address.FullAddress())
            .RuleFor(o => o.Product, f => f.Commerce.Product())
            .RuleFor(o => o.Price, f => f.Random.Decimal());

        var orderCreateUpdateDtoFaker = new Faker<OrderCreateUpdateDto>()
            .RuleFor(o => o.CustomerName, f => f.Name.FullName())
            .RuleFor(o => o.Address, f => f.Address.FullAddress())
            .RuleFor(o => o.Product, f => f.Commerce.Product())
            .RuleFor(o => o.Price, f => f.Random.Decimal());

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
