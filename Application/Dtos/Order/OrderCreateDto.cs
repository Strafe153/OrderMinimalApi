namespace Application.Dtos.Order;

public record OrderCreateDto(
    string CustomerName,
    string Address,
    string Product,
    decimal Price);