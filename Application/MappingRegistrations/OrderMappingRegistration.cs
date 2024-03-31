using Application.Dtos;
using Domain.Entities;
using Mapster;

namespace Application.MappingRegistrations;

public class OrderMappingRegistration : IRegister
{
	public void Register(TypeAdapterConfig config)
	{
		config.NewConfig<Order, OrderReadDto>();
		config.NewConfig<OrderCreateUpdateDto, Order>();
	}
}
