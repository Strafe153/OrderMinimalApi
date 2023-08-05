using Mapster;
using MinimalApi.Dtos;
using MinimalApi.Entities;

namespace MinimalApi.MappingRegistrations;

public class OrderMappingRegistration : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Order, OrderReadDto>();
        config.NewConfig<OrderCreateUpdateDto, Order>();
    }
}
