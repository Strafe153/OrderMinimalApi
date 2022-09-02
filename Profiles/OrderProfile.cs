using AutoMapper;
using OrderMinimalApi.Models;
using OrderMinimalApi.Dtos;

namespace OrderMinimalApi.Profiles
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<Order, OrderReadDto>();
            CreateMap<OrderCreateUpdateDto, Order>();
        }
    }
}
