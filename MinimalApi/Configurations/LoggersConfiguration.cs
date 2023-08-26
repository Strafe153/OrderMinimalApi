using Serilog;
using Serilog.Events;
using Serilog.Exceptions;

namespace MinimalApi.Configurations;

public static class LoggersConfiguration
{
    public static void ConfigureLoggers(this WebApplicationBuilder builder)
    {
        var logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .MinimumLevel.Override("System", LogEventLevel.Warning)
            .Enrich.FromLogContext()
            .Enrich.WithExceptionDetails()
            .WriteTo.Console()
            .WriteTo.Seq(builder.Configuration.GetConnectionString("SeqConnection")!)
            .CreateLogger();

        builder.Logging
            .ClearProviders()
            .AddSerilog(logger);
    }
}
