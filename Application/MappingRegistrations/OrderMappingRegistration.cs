using Application.Dtos.Order;
using Domain.Entities;
using Mapster;

namespace Application.MappingRegistrations;

public class OrderMappingRegistration : IRegister
{
	public void Register(TypeAdapterConfig config)
	{
		config.NewConfig<Order, OrderReadDto>();
		config.NewConfig<OrderCreateDto, Order>();
		config.NewConfig<OrderUpdateDto, Order>();
	}
}
