namespace Api.Features.Orders.UpdateOrder;

public record UpdateOrderRequest(
    string CustomerName,
	string Address,
	string Product,
	decimal Price);
