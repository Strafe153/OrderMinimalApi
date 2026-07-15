using Api.Constants;
using Api.Entities;
using Api.Exceptions;
using Api.Filters;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace Api.Features.Orders.CreateOrder;

public class CreateOrderEndpoint
{
    public static void Map(IEndpointRouteBuilder builder)
    {
        builder
            .MapPost(string.Empty, Handle)
            .AddEndpointFilter<ValidationFilter<CreateOrderRequest>>();
    }

    public static async Task<Created<CreateOrderResponse>> Handle(
        [FromBody] CreateOrderRequest request,
        [FromServices] IOrdersRepository repository,
        [FromServices] IOutputCacheStore outputCache,
        [FromServices] ILogger<CreateOrderEndpoint> logger,
        CancellationToken token)
    {
        Order order = new()
        {
            CustomerName = request.CustomerName,
            Address = request.Address,
            Product = request.Product,
            Price = request.Price
        };

        try
        {
            await repository.CreateAsync(order);
            await outputCache.EvictByTagAsync(CacheConstants.OrdersTag, token);
        }
        catch
        {
            logger.LogWarning("Failed to create the order.");
            throw new OperationFailedException($"Failed to create the order.");
        }

        CreateOrderResponse response = new(
            order.Id,
            order.CustomerName,
            order.Address,
            order.Product,
            order.Price);

        return TypedResults.Created($"api/orders/{response.Id}", response);
    }
}
