using Microsoft.AspNetCore.RateLimiting;
using OrderMinimalApi.Configurations.ConfigurationModels;
using System.Threading.RateLimiting;

namespace OrderMinimalApi.Configurations;

public static class RateLimitingConfiguration
{
    public static void ConfigureRateLimiting(this IServiceCollection services, IConfiguration configuration)
    {
        var rateLimitOptions = new RateLimitOptions();

        configuration
            .GetSection(RateLimitOptions.RateLimitSettingsSectionName)
            .Bind(rateLimitOptions);

        services.AddRateLimiter(options =>
        {
            options.AddTokenBucketLimiter("tokenBucket", tokenOptions =>
            {
                tokenOptions.TokenLimit = rateLimitOptions.TokenLimit;
                tokenOptions.TokensPerPeriod = rateLimitOptions.TokensPerPeriod;
                tokenOptions.ReplenishmentPeriod = TimeSpan.FromSeconds(rateLimitOptions.ReplenishmentPeriod);
                tokenOptions.AutoReplenishment = rateLimitOptions.AutoReplenishment;
                tokenOptions.QueueLimit = rateLimitOptions.QueueLimit;
                tokenOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
            });

            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

            options.OnRejected = (context, _) =>
            {
                context.HttpContext.Response.WriteAsync("Too many requests. Please try again later.");
                return new ValueTask();
            };
        });
    }
}
