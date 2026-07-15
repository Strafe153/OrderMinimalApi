namespace Api.Features.Orders.CreateOrder;

public record CreateOrderResponse(
    string Id,
    string CustomerId,
    string Address,
    string Product,
    decimal Price);
