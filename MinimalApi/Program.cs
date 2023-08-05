using MinimalApi.Configurations;
using MinimalApi.Endpoints;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureDatabase(builder.Configuration);
builder.Services.ConfigureHealthChecks(builder.Configuration);
builder.Services.ConfigureRateLimiting(builder.Configuration);

builder.Services.AddRepositories();
builder.Services.AddCustomServices();
builder.Services.AddCustomValidators();

builder.Services.AddMemoryCache();

builder.Services.ConfigureMapster();
builder.Services.ConfigureFluentValidation();

builder.Services.ConfigureApiVersioning();
builder.Services.ConfigureSwagger();

var app = builder.Build();

app.UseCustomMiddleware();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.ConfigureSwaggerUI();
}

app.UseHttpsRedirection();

app.UseHealthChecks();

app.UseRateLimiter();

// Add Order endpoints.
app.UseOrderEndpoints();

app.Run();