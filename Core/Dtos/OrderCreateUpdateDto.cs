namespace Core.Dtos;

public record OrderCreateUpdateDto
{
    public string CustomerName { get; init; } = default!;
    public string Address { get; init; } = default!;
    public string Product { get; init; } = default!;
    public decimal Price { get; init; }
}
