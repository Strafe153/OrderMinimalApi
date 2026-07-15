namespace Api.Features.Orders.GetOrder;

public record GetOrderResponse(
    string Id,
    string CustomerId,
    string Address,
    string Product,
    decimal Price);
