using FluentValidation;
using FluentValidation.AspNetCore;
using OrderMinimalApi.Dtos;
using OrderMinimalApi.Validators;

namespace OrderMinimalApi.Configurations;

public static class FluentValidationConfiguration
{
    public static void ConfigureFluentValidation(this IServiceCollection services)
    {
        services.AddFluentValidationAutoValidation()
            .AddFluentValidationClientsideAdapters();
    }

    public static void AddCustomValidators(this IServiceCollection services)
    {
        services.AddScoped<IValidator<OrderCreateUpdateDto>, OrderCreateUpdateValidator>();
    }
}
