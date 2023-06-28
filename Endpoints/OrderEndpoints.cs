using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using OrderMinimalApi.Dtos;
using OrderMinimalApi.Extensions;
using OrderMinimalApi.Services;

namespace OrderMinimalApi.Endpoints;

public static class OrderEndpoints
{
    public static void MapOrderEndpoints(this WebApplication app)
    {
        app.MapGet("v{version:apiVersion}/api/orders", GetAllAsync);
        app.MapGet("v{version:apiVersion}/api/orders/{id}", GetAsync);
        app.MapPost("v{version:apiVersion}/api/orders", CreateAsync);
        app.MapPut("v{version:apiVersion}/api/orders/{id}", UpdateAsync);
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
        [FromServices] IValidator<OrderCreateUpdateDto> validator,
        [FromBody] OrderCreateUpdateDto createDto)
    {
        var validationResult = validator.Validate(createDto);

        if (validationResult.IsValid)
        {
            var readDto = await service.CreateAsync(createDto);
            return Results.Created($"api/orders/{readDto.Id}", readDto);
        }

        var failuresDictionary = validationResult.Errors.ToDictionary();

        return Results.ValidationProblem(failuresDictionary);
    }

    public static async Task<IResult> UpdateAsync(
        [FromServices] IOrderService service,
        [FromServices] IValidator<OrderCreateUpdateDto> validator,
        [FromRoute] string id, 
        [FromBody] OrderCreateUpdateDto updateDto)
    {
        var validationResult = validator.Validate(updateDto);

        if (validationResult.IsValid)
        {
            await service.UpdateAsync(id, updateDto);
            return Results.NoContent();
        }

        var failuresDictionary = validationResult.Errors.ToDictionary();

        return Results.ValidationProblem(failuresDictionary);
    }

    public static async Task<IResult> DeleteAsync(
        [FromServices] IOrderService service,
        [FromRoute] string id)
    {
        await service.DeleteAsync(id);
        return Results.NoContent();
    }
}
