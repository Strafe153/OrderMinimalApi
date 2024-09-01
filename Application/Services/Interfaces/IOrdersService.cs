using Application.Dtos.Order;

namespace Application.Services.Interfaces;

public interface IOrdersService
{
	Task<IEnumerable<OrderReadDto>> GetAllAsync(CancellationToken token);
	Task<OrderReadDto> GetByIdAsync(string id, CancellationToken token);
	Task<OrderReadDto> CreateAsync(OrderCreateDto dto, CancellationToken token);
	Task UpdateAsync(string id, OrderUpdateDto dto, CancellationToken token);
	Task DeleteAsync(string id, CancellationToken token);
}
