using Application.Dtos.Order;
using Domain.Entities;

namespace Application.MappingRegistrations;

public static class OrderMappings
{
	extension(Order order)
	{
		public OrderReadDto ToReadDto() => new(
			order.Id,
			order.CustomerName,
			order.Address,
			order.Product,
			order.Price);
	}

	extension(IEnumerable<Order> orders)
	{
		public List<OrderReadDto> ToReadDto() => [.. orders.Select(ToReadDto)];
	}

	extension(OrderReadDto dto)
	{
		public Order ToOrder() => new()
		{
			CustomerName = dto.CustomerId,
			Address = dto.Address,
			Product = dto.Product,
			Price = dto.Price
		};
	}

	extension(OrderCreateDto dto)
	{
		public Order ToOrder() => new()
		{
			CustomerName = dto.CustomerName,
			Address = dto.Address,
			Product = dto.Product,
			Price = dto.Price
		};
	}

	extension(OrderUpdateDto dto)
	{
		public void Update(Order order)
		{
			order.CustomerName = dto.CustomerName;
			order.Address = dto.Address;
			order.Product = dto.Product;
			order.Price = dto.Price;
		}
	}
}