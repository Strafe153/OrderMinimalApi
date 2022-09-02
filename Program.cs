using AutoMapper;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using OrderMinimalApi.Dtos;
using OrderMinimalApi.Extensions;
using OrderMinimalApi.Middleware;
using OrderMinimalApi.Models;
using OrderMinimalApi.Repositories;
using OrderMinimalApi.Services;
using OrderMinimalApi.Validators;

var builder = WebApplication.CreateBuilder(args);

// Configure MongoDb database.
builder.Services.Configure<OrderDatabaseSettings>(
    builder.Configuration.GetSection("OrderDatabase"));

// Add repositories and services.
builder.Services.AddSingleton<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IValidator<OrderCreateUpdateDto>, OrderCreateUpdateValidator>();

// Add AutoMapper.
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Add FluentValidation.
builder.Services
    .AddFluentValidationAutoValidation()
    .AddFluentValidationClientsideAdapters();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Add ExceptionMiddleware.
app.UseMiddleware<ExceptionMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("api/orders", async (IOrderService service, IMapper mapper) =>
{
    var orders = await service.GetAllAsync();
    var readDtos = mapper.Map<IEnumerable<OrderReadDto>>(orders);

    return Results.Ok(readDtos);
});

app.MapGet("api/orders/{id}", async (IOrderService service, IMapper mapper, [FromRoute] string id) =>
{
    var order = await service.GetByIdAsync(id);
    var readDto = mapper.Map<OrderReadDto>(order);

    return Results.Ok(readDto);
});

app.MapPost("api/orders", async (IOrderService service, IMapper mapper, IValidator<OrderCreateUpdateDto> validator, [FromBody] OrderCreateUpdateDto createDto) =>
{
    var validationResult = validator.Validate(createDto);

    if (validationResult.IsValid)
    {
        var order = mapper.Map<Order>(createDto);
        await service.CreateAsync(order);

        var readDto = mapper.Map<OrderReadDto>(order);

        return Results.Created($"api/orders/{order.Id}", readDto);
    }

    var failuresDictionary = validationResult.Errors.ToDictionaryOfStringArrays();

    return Results.ValidationProblem(failuresDictionary);
});

app.MapPut("api/orders/{id}", async (IOrderService service, IMapper mapper, IValidator<OrderCreateUpdateDto> validator, [FromRoute] string id, [FromBody] OrderCreateUpdateDto updateDto) =>
{
    var validationResult = validator.Validate(updateDto);

    if (validationResult.IsValid)
    {
        var order = await service.GetByIdAsync(id);

        mapper.Map(updateDto, order);
        await service.UpdateAsync(id, order);

        return Results.NoContent();
    }

    var failuresDictionary = validationResult.Errors.ToDictionaryOfStringArrays();

    return Results.ValidationProblem(failuresDictionary);
});

app.MapDelete("api/order/{id}", async (IOrderService service, IMapper mapper, [FromRoute] string id) =>
{
    await service.DeleteAsync(id);
    return Results.NoContent();
});

app.Run();