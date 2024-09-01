using Application.Dtos.Order;
using Application.Services.Interfaces;
using Domain.Shared.Constants;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MinimalApi.Filters;
using OpenIddict.Validation.AspNetCore;

namespace MinimalApi.Endpoints;

public static class OrderEndpoints
{
	public static void MapOrderEndpoints(this WebApplication app)
	{
		var orderEndpointsGroup = app
			.MapGroup("api/v{version:apiVersion}/orders")
			.RequireRateLimiting(RateLimitingConstants.TokenBucket)
			.RequireAuthorization();

		orderEndpointsGroup
			.MapGet(string.Empty, GetAll)
			.CacheOutput(p => p.Tag(CacheConstants.OrdersTag));

		orderEndpointsGroup
			.MapGet("{id}", Get)
			.CacheOutput(p => p.Tag(CacheConstants.OrderTag));

		orderEndpointsGroup
			.MapPost(string.Empty, Create)
			.AddEndpointFilter<ValidationFilter<OrderCreateDto>>();

		orderEndpointsGroup
			.MapPut("{id}", Update)
			.AddEndpointFilter<ValidationFilter<OrderUpdateDto>>();

		orderEndpointsGroup.MapDelete("{id}", Delete);
	}

	public static async Task<Ok<IEnumerable<OrderReadDto>>> GetAll(
		[FromServices] IOrdersService service,
		CancellationToken token) =>
			TypedResults.Ok(await service.GetAllAsync(token));

	public static async Task<Ok<OrderReadDto>> Get(
		[FromServices] IOrdersService service,
		[FromRoute] string id,
		CancellationToken token) =>
			TypedResults.Ok(await service.GetByIdAsync(id, token));

	public static async Task<Created<OrderReadDto>> Create(
		[FromServices] IOrdersService service,
		[FromBody] OrderCreateDto createDto,
		CancellationToken token)
	{
		var readDto = await service.CreateAsync(createDto, token);
		return TypedResults.Created($"api/orders/{readDto.Id}", readDto);
	}

	public static async Task<NoContent> Update(
		[FromServices] IOrdersService service,
		[FromRoute] string id,
		[FromBody] OrderUpdateDto updateDto,
		CancellationToken token)
	{
		await service.UpdateAsync(id, updateDto, token);
		return TypedResults.NoContent();
	}

	public static async Task<NoContent> Delete(
		[FromServices] IOrdersService service,
		[FromRoute] string id,
		CancellationToken token)
	{
		await service.DeleteAsync(id, token);
		return TypedResults.NoContent();
	}
}
