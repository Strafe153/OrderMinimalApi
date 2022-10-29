using FluentValidation;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using OrderMinimalApi.Dtos;
using OrderMinimalApi.Extensions;
using OrderMinimalApi.Services;
using OrderMinimalApi.Shared;

namespace OrderMinimalApi.Endpoints;

public static class OrderEndpoints
{
    public static void MapOrderEndpoints(this WebApplication app)
    {
        app.MapGet("api/orders", GetAllAsync);
        app.MapGet("api/orders/{id}", GetAsync);
        app.MapPost("api/orders", CreateAsync);
        app.MapPut("api/orders/{id}", UpdateAsync);
        app.MapDelete("api/orders/{id}", DeleteAsync);
    }

    public static async Task<IResult> GetAllAsync(IOrderService service, IMapper mapper, CancellationToken token)
    {
        var orders = await service.GetAllAsync(token);
        var readDtos = mapper.Map<IEnumerable<OrderReadDto>>(orders);

        return Results.Ok(readDtos);
    }

    public static async Task<IResult> GetAsync(IOrderService service, IMapper mapper, [FromRoute] string id, CancellationToken token)
    {
        var order = await service.GetByIdAsync(id, token);
        var readDto = mapper.Map<OrderReadDto>(order);

        return Results.Ok(readDto);
    }

    public static async Task<IResult> CreateAsync(IOrderService service, IMapper mapper, IValidator<OrderCreateUpdateDto> validator,
        [FromBody] OrderCreateUpdateDto createDto)
    {
        var validationResult = validator.Validate(createDto);

        if (validationResult.IsValid)
        {
            var order = mapper.Map<Order>(createDto);
            await service.CreateAsync(order);

            var readDto = mapper.Map<OrderReadDto>(order);

            return Results.Created($"api/orders/{order.Id}", readDto);
        }

        var failuresDictionary = validationResult.Errors.ToDictionary();

        return Results.ValidationProblem(failuresDictionary);
    }

    public static async Task<IResult> UpdateAsync(IOrderService service, IMapper mapper, IValidator<OrderCreateUpdateDto> validator,
        [FromRoute] string id, [FromBody] OrderCreateUpdateDto updateDto)
    {
        var validationResult = validator.Validate(updateDto);

        if (validationResult.IsValid)
        {
            var order = await service.GetByIdAsync(id);

            mapper.Map(updateDto, order);
            await service.UpdateAsync(id, order);

            return Results.NoContent();
        }

        var failuresDictionary = validationResult.Errors.ToDictionary();

        return Results.ValidationProblem(failuresDictionary);
    }

    public static async Task<IResult> DeleteAsync(IOrderService service, IMapper mapper, [FromRoute] string id)
    {
        await service.DeleteAsync(id);
        return Results.NoContent();
    }
}
