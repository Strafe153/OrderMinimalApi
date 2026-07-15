using Api.Constants;
using Api.Exceptions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace Api.Features.Orders.DeleteOrder;

public class DeleteOrderEndpoint
{
    public static void Map(IEndpointRouteBuilder builder)
    {
        builder.MapDelete("{id}", Handle);
    }

    public static async Task<NoContent> Handle(
        [FromRoute] string id,
        [FromServices] IOrdersRepository repository,
        [FromServices] IOutputCacheStore outputCache,
        [FromServices] ILogger<DeleteOrderEndpoint> logger,
        CancellationToken token)
    {
        var order = await repository.GetByIdAsync(id, token);

        if (order is null)
        {
            logger.LogWarning("The order with id='{Id}' not found.", id);
            throw new NullReferenceException($"Order with id '{id}' not found.");
        }

        try
		{
		    await repository.DeleteAsync(id, token);
		}
		catch
		{
			logger.LogWarning("Failed to update the order with id='{Id}'.", id);
			throw new OperationFailedException($"Failed to delete the order with id={id}.");
		}

		await outputCache.EvictByTagAsync(CacheConstants.OrdersTag, token);
		await outputCache.EvictByTagAsync(CacheConstants.OrderTag, token);

		logger.LogInformation("Successfully deleted the order with id='{Id}'", id);

        return TypedResults.NoContent();
    }
}
