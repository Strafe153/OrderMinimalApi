﻿using Mapster;
using OrderMinimalApi.Dtos;
using OrderMinimalApi.Models;

namespace OrderMinimalApi.MappingRegistrations
{
    public class OrderMappingRegistration : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Order, OrderReadDto>();
            config.NewConfig<OrderCreateUpdateDto, Order>();
        }
    }
}
