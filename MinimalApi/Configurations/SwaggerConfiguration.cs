using Asp.Versioning.ApiExplorer;
using Microsoft.OpenApi.Models;

namespace MinimalApi.Configurations;

public static class SwaggerConfiguration
{
    public static void ConfigureSwagger(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen(options =>
        {
            var apiVersionDescriptionProvider = services
                .BuildServiceProvider()
                .GetRequiredService<IApiVersionDescriptionProvider>();

            foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(description.GroupName, new OpenApiInfo
                {
                    Title = $"{typeof(Program).Assembly.GetName().Name} {description.ApiVersion}",
                    Version = description.ApiVersion.ToString()
                });
            }
        });
    }

    public static void ConfigureSwaggerUI(this WebApplication application)
    {
        application.UseSwagger();

        application.UseSwaggerUI(options =>
        {
            var provider = application.Services.GetRequiredService<IApiVersionDescriptionProvider>();

            foreach (var description in provider.ApiVersionDescriptions)
            {
                options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
            }
        });
    }
}
