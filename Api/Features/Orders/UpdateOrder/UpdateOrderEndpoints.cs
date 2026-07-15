using Api.Constants;
using Api.Exceptions;
using Api.Filters;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace Api.Features.Orders.UpdateOrder;

public class UpdateOrderEndpoint
{
    public static void Map(IEndpointRouteBuilder builder)
    {
        builder
            .MapPut("{id}", Handle)
            .AddEndpointFilter<ValidationFilter<UpdateOrderRequest>>();
    }

    public static async Task<NoContent> Handle(
        [FromRoute] string id,
        [FromBody] UpdateOrderRequest request,
        [FromServices] IOrdersRepository repository,
        [FromServices] IOutputCacheStore outputCache,
        [FromServices] ILogger<UpdateOrderEndpoint> logger,
        CancellationToken token)
    {
        var order = await repository.GetByIdAsync(id, token);

        if (order is null)
        {
            logger.LogWarning("The order with id='{Id}' not found.", id);
            throw new NullReferenceException($"Order with id '{id}' not found.");
        }

        order.CustomerName = request.CustomerName;
        order.Address = request.Address;
        order.Product = request.Product;
        order.Price = request.Price;

        try
        {
            await repository.UpdateAsync(id, order);
            logger.LogInformation("Successfully updated the order with id='{Id}'", id);
        }
        catch
        {
            logger.LogWarning("Failed to update the order with id='{Id}'.", id);
            throw new OperationFailedException($"Failed to update the order with id={id}.");
        }

        await outputCache.EvictByTagAsync(CacheConstants.OrdersTag, token);
        await outputCache.EvictByTagAsync(CacheConstants.OrderTag, token);

        return TypedResults.NoContent();
    }
}
