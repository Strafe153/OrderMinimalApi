﻿using OrderMinimalApi.Dtos;
using OrderMinimalApi.Shared;

namespace OrderMinimalApi.Services;

public interface IOrderService
{
    Task<IEnumerable<OrderReadDto>> GetAllAsync(CancellationToken token = default);
    Task<OrderReadDto> GetByIdAsync(string id, CancellationToken token = default);
    Task<OrderReadDto> CreateAsync(OrderCreateUpdateDto dto);
    Task UpdateAsync(string id, OrderCreateUpdateDto newOrderDto);
    Task DeleteAsync(string id);
}
