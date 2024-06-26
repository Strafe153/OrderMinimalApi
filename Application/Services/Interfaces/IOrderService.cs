﻿using Application.Dtos;

namespace Application.Services.Interfaces;

public interface IOrderService
{
	Task<IEnumerable<OrderReadDto>> GetAllAsync(CancellationToken token = default);
	Task<OrderReadDto> GetByIdAsync(string id, CancellationToken token = default);
	Task<OrderReadDto> CreateAsync(OrderCreateUpdateDto dto);
	Task UpdateAsync(string id, OrderCreateUpdateDto dto);
	Task DeleteAsync(string id);
}
