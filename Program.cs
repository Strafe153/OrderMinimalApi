using FluentValidation;
using FluentValidation.AspNetCore;
using OrderMinimalApi.Dtos;
using OrderMinimalApi.Endpoints;
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

// Add repositories and services, validators
builder.Services.AddSingleton<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IValidator<OrderCreateUpdateDto>, OrderCreateUpdateValidator>();

// Add MemoryCache.
builder.Services.AddMemoryCache();

// Add Mapster.
builder.Services.AddMapster();

// Add FluentValidation.
builder.Services
    .AddFluentValidationAutoValidation()
    .AddFluentValidationClientsideAdapters();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Add custom middleware.
app.AddApplicationMiddleware();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Add Order endpoints.
app.MapOrderEndpoints();

app.Run();