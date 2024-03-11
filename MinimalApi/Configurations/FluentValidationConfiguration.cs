using Domain.Dtos;
using FluentValidation;
using FluentValidation.AspNetCore;
using MinimalApi.Validation.Validators;

namespace MinimalApi.Configurations;

public static class FluentValidationConfiguration
{
	public static void ConfigureFluentValidation(this IServiceCollection services) =>
		services
			.AddFluentValidationAutoValidation()
			.AddFluentValidationClientsideAdapters();

	public static void AddCustomValidators(this IServiceCollection services) =>
		services.AddScoped<IValidator<OrderCreateUpdateDto>, OrderCreateUpdateValidator>();
}
