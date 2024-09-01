namespace Application.Dtos.Order;

public record OrderUpdateDto(
	string CustomerName,
	string Address,
	string Product,
	decimal Price);