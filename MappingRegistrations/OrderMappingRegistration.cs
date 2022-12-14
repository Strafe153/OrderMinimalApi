using Mapster;
using OrderMinimalApi.Dtos;
using OrderMinimalApi.Shared;

namespace OrderMinimalApi.MappingRegistrations;

public class OrderMappingRegistration : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Order, OrderReadDto>();
        config.NewConfig<OrderCreateUpdateDto, Order>();
    }
}
