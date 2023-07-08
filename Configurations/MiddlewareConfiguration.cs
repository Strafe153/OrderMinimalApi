using OrderMinimalApi.Middleware;

namespace OrderMinimalApi.Configurations;

public static class MiddlewareConfiguration
{
    public static void UseCustomMiddleware(this IApplicationBuilder builder)
    {
        builder.UseMiddleware<ExceptionHandlingMiddleware>();
    }
}
