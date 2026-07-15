using Api.Constants;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace Api.Features.Orders.GetOrders;

public class GetOrdersEndpoint
{
    public static void Map(IEndpointRouteBuilder builder)
    {
        builder
            .MapGet(string.Empty, Handle)
            .CacheOutput(p => p.Tag(CacheConstants.OrdersTag));
    }

    public static async Task<Ok<GetOrdersResponse>> Handle(
        [FromServices] IOrdersRepository repository,
        [FromServices] ILogger<GetOrdersEndpoint> logger,
        CancellationToken token)
    {
        var orders = await repository.GetAllAsync(token);

        logger.LogInformation("Successfully retrieved all the orders.");

        var responseOrders = orders
            .Select(o => new GetOrdersResponseOrder(
                o.Id,
                o.CustomerName,
                o.Address,
                o.Product,
                o.Price))
            .ToList();

        GetOrdersResponse response = new(responseOrders.Count, responseOrders);

        return TypedResults.Ok(response);
    }
}
