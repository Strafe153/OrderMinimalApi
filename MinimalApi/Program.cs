using MinimalApi;
using MinimalApi.Configurations;
using MinimalApi.Endpoints;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureLoggers();

builder.Services.ConfigureDatabase(builder.Configuration);
builder.Services.ConfigureHealthChecks(builder.Configuration);
builder.Services.ConfigureRateLimiting(builder.Configuration);
builder.Services.ConfigureHttpClients(builder.Configuration);

builder.Services.AddRepositories();
builder.Services.AddCustomServices();
builder.Services.AddCustomValidators();

builder.Services.ConfigureOutputCache(builder.Configuration);

builder.Services.ConfigureMapster();
builder.Services.ConfigureFluentValidation();

builder.Services.AddExceptionHandler<ExceptionHandler>();

builder.Services.ConfigureApiVersioning();
builder.Services.ConfigureSwagger();

var app = builder.Build();

// As of now that's a confirmed, though yet unfixed bug acknowledged by Microsoft
app.UseExceptionHandler(_ => { });

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.ConfigureSwaggerUI();
}

app.UseHttpsRedirection();

app.UseHealthChecks();

app.UseRateLimiter();

app.UseOutputCache();

// Add Order endpoints.
app.UseOrderEndpoints();

app.Run();