using Core.Shared;
using Microsoft.AspNetCore.RateLimiting;
using MinimalApi.Configurations.ConfigurationModels;

namespace MinimalApi.Configurations;

public static class RateLimitingConfiguration
{
    public static void ConfigureRateLimiting(this IServiceCollection services, IConfiguration configuration)
    {
        var rateLimitOptions = configuration
            .GetSection(RateLimitOptions.RateLimitOptionsSectionName)
            .Get<RateLimitOptions>()!;

        services.AddRateLimiter(options =>
        {
            options.AddTokenBucketLimiter(RateLimitingConstants.TokenBucket, tokenOptions =>
            {
                tokenOptions.TokenLimit = rateLimitOptions.TokenLimit;
                tokenOptions.TokensPerPeriod = rateLimitOptions.TokensPerPeriod;
                tokenOptions.ReplenishmentPeriod = TimeSpan.FromSeconds(rateLimitOptions.ReplenishmentPeriod);
                tokenOptions.AutoReplenishment = rateLimitOptions.AutoReplenishment;
                tokenOptions.QueueLimit = rateLimitOptions.QueueLimit;
                tokenOptions.QueueProcessingOrder = rateLimitOptions.QueueProcessingOrder;
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
