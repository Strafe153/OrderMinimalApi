using OrderMinimalApi.Middleware;

namespace OrderMinimalApi.Configurations;

public static class MiddlewareConfiguration
{
    public static void AddCustomMiddleware(this IApplicationBuilder builder)
    {
        builder.UseMiddleware<ExceptionHandlingMiddleware>();
    }
}
