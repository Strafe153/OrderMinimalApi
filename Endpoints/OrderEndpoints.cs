using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using OrderMinimalApi.Dtos;
using OrderMinimalApi.Filters;
using OrderMinimalApi.Services;

namespace OrderMinimalApi.Endpoints;

public static class OrderEndpoints
{
    public static void MapOrderEndpoints(this WebApplication app)
    {
        var orderEndpointsGroup = app
            .MapGroup("api/v{version:apiVersion}/orders")
            .RequireRateLimiting("tokenBucket");

        orderEndpointsGroup.MapGet(string.Empty, GetAllAsync);

        orderEndpointsGroup.MapGet("{id}", GetAsync);

        orderEndpointsGroup.MapPost(string.Empty, CreateAsync)
            .AddEndpointFilter<ValidationFilter<OrderCreateUpdateDto>>();

        orderEndpointsGroup.MapPut("{id}", UpdateAsync)
            .AddEndpointFilter<ValidationFilter<OrderCreateUpdateDto>>();

        orderEndpointsGroup.MapDelete("{id}", DeleteAsync);
    }

    public static async Task<Ok<IEnumerable<OrderReadDto>>> GetAllAsync(
        [FromServices] IOrderService service,
        CancellationToken token) =>
            TypedResults.Ok(await service.GetAllAsync(token));

    public static async Task<Ok<OrderReadDto>> GetAsync(
        [FromServices] IOrderService service,
        [FromRoute] string id,
        CancellationToken token) =>
            TypedResults.Ok(await service.GetByIdAsync(id, token));

    public static async Task<Created<OrderReadDto>> CreateAsync(
        [FromServices] IOrderService service,
        [FromBody] OrderCreateUpdateDto createDto)
    {
        var readDto = await service.CreateAsync(createDto);
        return TypedResults.Created($"api/orders/{readDto.Id}", readDto);
    }

    public static async Task<NoContent> UpdateAsync(
        [FromServices] IOrderService service,
        [FromRoute] string id, 
        [FromBody] OrderCreateUpdateDto updateDto)
    {
        await service.UpdateAsync(id, updateDto);
        return TypedResults.NoContent();
    }

    public static async Task<NoContent> DeleteAsync(
        [FromServices] IOrderService service,
        [FromRoute] string id)
    {
        await service.DeleteAsync(id);
        return TypedResults.NoContent();
    }
}
