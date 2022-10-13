namespace OrderMinimalApi.Dtos;

public record OrderReadDto
{
    public string Id { get; init; } = default!;
    public string CustomerName { get; init; } = default!;
    public string Address { get; init; } = default!;
    public string Product { get; init; } = default!;
    public decimal Price { get; init; }
}
