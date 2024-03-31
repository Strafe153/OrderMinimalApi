namespace Application.Dtos;

public record OrderReadDto(
	string Id,
	string CustomerName,
	string Address,
	string Product,
	decimal Price);
