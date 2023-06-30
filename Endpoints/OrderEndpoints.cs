using Microsoft.AspNetCore.Mvc;
using OrderMinimalApi.Dtos;
using OrderMinimalApi.Filters;
using OrderMinimalApi.Services;

namespace OrderMinimalApi.Endpoints;

public static class OrderEndpoints
{
    public static void MapOrderEndpoints(this WebApplication app)
    {
        app.MapGet("v{version:apiVersion}/api/orders", GetAllAsync);
        app.MapGet("v{version:apiVersion}/api/orders/{id}", GetAsync);

        app.MapPost("v{version:apiVersion}/api/orders", CreateAsync)
            .AddEndpointFilter<ValidationFilter<OrderCreateUpdateDto>>();

        app.MapPut("v{version:apiVersion}/api/orders/{id}", UpdateAsync)
            .AddEndpointFilter<ValidationFilter<OrderCreateUpdateDto>>();

        app.MapDelete("v{version:apiVersion}/api/orders/{id}", DeleteAsync);
    }

    public static async Task<IResult> GetAllAsync(
        [FromServices] IOrderService service,
        CancellationToken token) =>
            Results.Ok(await service.GetAllAsync(token));

    public static async Task<IResult> GetAsync(
        [FromServices] IOrderService service,
        [FromRoute] string id,
        CancellationToken token) =>
            Results.Ok(await service.GetByIdAsync(id, token));

    public static async Task<IResult> CreateAsync(
        [FromServices] IOrderService service,
        [FromBody] OrderCreateUpdateDto createDto)
    {
        var readDto = await service.CreateAsync(createDto);
        return Results.Created($"api/orders/{readDto.Id}", readDto);
    }

    public static async Task<IResult> UpdateAsync(
        [FromServices] IOrderService service,
        [FromRoute] string id, 
        [FromBody] OrderCreateUpdateDto updateDto)
    {
        await service.UpdateAsync(id, updateDto);
        return Results.NoContent();
    }

    public static async Task<IResult> DeleteAsync(
        [FromServices] IOrderService service,
        [FromRoute] string id)
    {
        await service.DeleteAsync(id);
        return Results.NoContent();
    }
}
