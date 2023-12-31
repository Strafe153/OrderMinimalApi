using Core.Shared.Constants;
using Serilog;
using Serilog.Events;
using Serilog.Exceptions;

namespace MinimalApi.Configurations;

public static class LoggerConfiguration
{
    public static void ConfigureLoggers(this WebApplicationBuilder builder)
    {
        var logger = new Serilog.LoggerConfiguration()
            .MinimumLevel.Information()
            .MinimumLevel.Override("System", LogEventLevel.Warning)
            .Enrich.FromLogContext()
            .Enrich.WithExceptionDetails()
            .WriteTo.Console()
            .WriteTo.Seq(builder.Configuration.GetConnectionString(ConnectionStringConstants.SeqConnection)!)
            .CreateLogger();

        builder.Logging
            .ClearProviders()
            .AddSerilog(logger);
    }
}
