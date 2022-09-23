namespace OrderMinimalApi.Dtos;

public record OrderReadDto
{
    public string? Id { get; init; }
    public string? CustomerName { get; init; }
    public string? Address { get; init; }
    public string? Product { get; init; }
    public decimal Price { get; init; }
}
