using Api.Constants;
using Api.Features.Orders.CreateOrder;
using Api.Features.Orders.DeleteOrder;
using Api.Features.Orders.GetOrder;
using Api.Features.Orders.GetOrders;
using Api.Features.Orders.UpdateOrder;

namespace Api.Features.Orders;

public static class OrderEndpointsRegistration
{
    public static void MapOrderEndpoints(this IEndpointRouteBuilder builder)
    {
        var orderEndpointsGroup = builder
			.MapGroup("api/v{version:apiVersion}/orders")
			.RequireRateLimiting(RateLimitingConstants.TokenBucket)
			.RequireAuthorization();

        GetOrderEndpoint.Map(orderEndpointsGroup);
        GetOrdersEndpoint.Map(orderEndpointsGroup);
        CreateOrderEndpoint.Map(orderEndpointsGroup);
        UpdateOrderEndpoint.Map(orderEndpointsGroup);
        DeleteOrderEndpoint.Map(orderEndpointsGroup);
    }
}