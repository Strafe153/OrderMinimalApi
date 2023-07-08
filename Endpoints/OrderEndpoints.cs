using Microsoft.AspNetCore.Mvc;
using OrderMinimalApi.Dtos;
using OrderMinimalApi.Filters;
using OrderMinimalApi.Services;

namespace OrderMinimalApi.Endpoints;

public static class OrderEndpoints
{
    public static void MapOrderEndpoints(this WebApplication app)
    {
        app.MapGet("api/v{version:apiVersion}/orders", GetAllAsync)
            .RequireRateLimiting("tokenBucket");

        app.MapGet("api/v{version:apiVersion}/orders/{id}", GetAsync)
            .RequireRateLimiting("tokenBucket");

        app.MapPost("api/v{version:apiVersion}/orders", CreateAsync)
            .AddEndpointFilter<ValidationFilter<OrderCreateUpdateDto>>()
            .RequireRateLimiting("tokenBucket");

        app.MapPut("api/v{version:apiVersion}/orders/{id}", UpdateAsync)
            .AddEndpointFilter<ValidationFilter<OrderCreateUpdateDto>>()
            .RequireRateLimiting("tokenBucket");

        app.MapDelete("api/v{version:apiVersion}/orders/{id}", DeleteAsync)
            .RequireRateLimiting("tokenBucket");
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
