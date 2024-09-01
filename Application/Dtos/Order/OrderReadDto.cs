namespace Application.Dtos.Order;

public record OrderReadDto(
    string Id,
    string CustomerId,
    string Address,
    string Product,
    decimal Price);
