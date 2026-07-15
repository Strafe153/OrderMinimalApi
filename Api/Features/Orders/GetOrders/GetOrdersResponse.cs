namespace Api.Features.Orders.GetOrders;

public record GetOrdersResponseOrder(
    string Id,
    string CustomerName,
    string Address,
    string Product,
    decimal Price);

public record GetOrdersResponse(int Count, List<GetOrdersResponseOrder> Items);
