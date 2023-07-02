using OrderMinimalApi.Configurations;
using OrderMinimalApi.Endpoints;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureDatabase(builder.Configuration);
builder.Services.ConfigureHealthChecks(builder.Configuration);

builder.Services.AddRepositories();
builder.Services.AddCustomServices();
builder.Services.AddCustomValidators();

builder.Services.AddMemoryCache();

builder.Services.ConfigureMapster();
builder.Services.ConfigureFluentValidation();

builder.Services.ConfigureApiVersioning();
builder.Services.ConfigureSwagger();

var app = builder.Build();

app.AddCustomMiddleware();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.ConfigureSwaggerUI();
}

app.UseHttpsRedirection();

app.UseHealthChecks();

// Add Order endpoints.
app.MapOrderEndpoints();

app.Run();