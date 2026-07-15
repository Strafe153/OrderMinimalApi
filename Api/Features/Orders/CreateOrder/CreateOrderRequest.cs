namespace Api.Features.Orders.CreateOrder;

public record CreateOrderRequest(
    string CustomerName,
    string Address,
    string Product,
    decimal Price);
