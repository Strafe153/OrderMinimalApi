using MinimalApi.Middleware;

namespace MinimalApi.Configurations;

public static class MiddlewareConfiguration
{
    public static void UseCustomMiddleware(this IApplicationBuilder builder)
    {
        builder.UseMiddleware<ExceptionHandlingMiddleware>();
    }
}
