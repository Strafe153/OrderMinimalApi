using System.Threading.RateLimiting;
using Api.Constants;
using Microsoft.AspNetCore.RateLimiting;

namespace Api.Configurations;

public static class RateLimitingConfiguration
{
	public static void ConfigureRateLimiting(this IServiceCollection services, IConfiguration configuration) =>
		services.AddRateLimiter(options =>
		{
			options.AddTokenBucketLimiter(RateLimitingConstants.TokenBucket, tokenOptions =>
			{
				var rateLimitOptions = configuration
					.GetSection(RateLimitingConstants.SectionName)
					.Get<TokenBucketRateLimiterOptions>()!;

				tokenOptions.TokenLimit = rateLimitOptions.TokenLimit;
				tokenOptions.TokensPerPeriod = rateLimitOptions.TokensPerPeriod;
				tokenOptions.ReplenishmentPeriod = rateLimitOptions.ReplenishmentPeriod;
				tokenOptions.AutoReplenishment = rateLimitOptions.AutoReplenishment;
				tokenOptions.QueueLimit = rateLimitOptions.QueueLimit;
				tokenOptions.QueueProcessingOrder = rateLimitOptions.QueueProcessingOrder;
			});

			options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

			options.OnRejected = (context, cancellationToken) =>
			{
				context.HttpContext.Response.WriteAsync("Too many requests. Please try again later.", cancellationToken);
				return new ValueTask();
			};
		});
}
