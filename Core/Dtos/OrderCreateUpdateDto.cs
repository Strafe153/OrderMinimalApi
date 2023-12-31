namespace Core.Dtos;

public record OrderCreateUpdateDto(
    string CustomerName,
    string Address,
    string Product,
    decimal Price);