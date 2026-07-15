using Api.Constants;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Api.Features.Orders.GetOrder;

public class GetOrderEndpoint
{
    public static void Map(IEndpointRouteBuilder builder)
    {
        builder
            .MapGet("{id}", Handle)
			.CacheOutput(p => p.Tag(CacheConstants.OrdersTag));
    }

    public static async Task<Ok<GetOrderResponse>> Handle(
        [FromRoute] string id,
        [FromServices] IOrdersRepository repository,
        [FromServices] ILogger<GetOrderEndpoint> logger,
        CancellationToken token)
    {
        var order = await repository.GetByIdAsync(id, token);

        if (order is null)
		{
			logger.LogWarning("The order with id='{Id}' not found.", id);
			throw new NullReferenceException($"Order with id '{id}' not found.");
		}

        logger.LogInformation("Successfully retrieved the order with id='{Id}'.", id);

        GetOrderResponse response = new(
            order.Id,
            order.CustomerName,
            order.Address,
            order.Product,
            order.Price);

        return TypedResults.Ok(response);
    }
}
