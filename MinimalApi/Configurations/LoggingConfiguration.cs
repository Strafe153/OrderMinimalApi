using MinimalApi.Configurations.Models;
using Serilog;
using Serilog.Events;
using Serilog.Exceptions;
using Serilog.Sinks.OpenTelemetry;

namespace MinimalApi.Configurations;

public static class LoggingConfiguration
{
    public static void ConfigureLoggers(this WebApplicationBuilder builder)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .MinimumLevel.Override(nameof(System), LogEventLevel.Warning)
            .Enrich.FromLogContext()
            .Enrich.WithExceptionDetails()
            .WriteTo.Console()
            .WriteTo.OpenTelemetry(options =>
            {
                var seqOptions = builder.Configuration.GetSection(SeqOptions.SectionName).Get<SeqOptions>()!;

                options.Endpoint = $"{seqOptions.ConnectionString}/ingest/otlp/v1/logs";
                options.Protocol = OtlpProtocol.HttpProtobuf;

                options.Headers = new Dictionary<string, string>
                {
                    { "X-Seq-ApiKey", seqOptions.ApiKey }
                };

                options.ResourceAttributes = new Dictionary<string, object>
                {
                    { "environment", builder.Environment.EnvironmentName }
                };
            })
            .CreateLogger();

        builder.Logging
            .ClearProviders()
            .AddSerilog();
    }
}
