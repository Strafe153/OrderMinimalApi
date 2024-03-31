using Application.Dtos;
using Application.Services.Interfaces;
using Domain.Shared.Constants;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MinimalApi.Filters;

namespace MinimalApi.Endpoints;

public static class OrderEndpoints
{
	public static void UseOrderEndpoints(this WebApplication app)
	{
		var orderEndpointsGroup = app
			.MapGroup("api/v{version:apiVersion}/orders")
			.RequireRateLimiting(RateLimitingConstants.TokenBucket);

		orderEndpointsGroup
			.MapGet(string.Empty, GetAll)
			.CacheOutput();

		orderEndpointsGroup
			.MapGet("{id}", Get)
			.CacheOutput();

		orderEndpointsGroup
			.MapPost(string.Empty, Create)
			.AddEndpointFilter<ValidationFilter<OrderCreateUpdateDto>>();

		orderEndpointsGroup
			.MapPut("{id}", Update)
			.AddEndpointFilter<ValidationFilter<OrderCreateUpdateDto>>();

		orderEndpointsGroup.MapDelete("{id}", Delete);
	}

	public static async Task<Ok<IEnumerable<OrderReadDto>>> GetAll([FromServices] IOrderService service, CancellationToken token) =>
		TypedResults.Ok(await service.GetAllAsync(token));

	public static async Task<Ok<OrderReadDto>> Get(
		[FromServices] IOrderService service,
		[FromRoute] string id,
		CancellationToken token) =>
			TypedResults.Ok(await service.GetByIdAsync(id, token));

	public static async Task<Created<OrderReadDto>> Create(
		[FromServices] IOrderService service,
		[FromBody] OrderCreateUpdateDto createDto)
	{
		var readDto = await service.CreateAsync(createDto);
		return TypedResults.Created($"api/orders/{readDto.Id}", readDto);
	}

	public static async Task<NoContent> Update(
		[FromServices] IOrderService service,
		[FromRoute] string id,
		[FromBody] OrderCreateUpdateDto updateDto)
	{
		await service.UpdateAsync(id, updateDto);
		return TypedResults.NoContent();
	}

	public static async Task<NoContent> Delete([FromServices] IOrderService service, [FromRoute] string id)
	{
		await service.DeleteAsync(id);
		return TypedResults.NoContent();
	}
}
