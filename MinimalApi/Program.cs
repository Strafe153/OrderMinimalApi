using MinimalApi;
using MinimalApi.Configurations;
using MinimalApi.Endpoints;
using OpenIddict.Validation.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureLoggers();

builder.Services.ConfigureDatabase(builder.Configuration);
builder.Services.ConfigureHealthChecks(builder.Configuration);
builder.Services.ConfigureRateLimiting(builder.Configuration);
builder.Services.ConfigureFlurl(builder.Configuration);

builder.Services.AddAuthentication(options =>
    options.DefaultScheme = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme);
builder.Services.AddAuthorization();
builder.Services.ConfigureOpenIddict(builder.Configuration);

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

if (app.Environment.IsDevelopment())
{
	app.ConfigureSwaggerUI();
}

app.UseHttpsRedirection();

app.UseHealthChecks();

app.UseRateLimiter();

app.UseOutputCache();

app.UseAuthentication();
app.UseAuthorization();

app.MapAuthorizationEndpoints();
app.MapOrderEndpoints();

await app.CreateIndexes();

app.Run();