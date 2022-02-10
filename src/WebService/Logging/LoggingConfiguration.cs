using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Extensions.Logging;

namespace WebService.Logging
{
    public static class LoggingConfiguration
    {
        public static void ConfigLogging(this WebApplicationBuilder hostBuilder)
        {
            hostBuilder.Host.ConfigureLogging(logging =>
            {
                logging.ConfigureLogging(hostBuilder.Configuration);
            });
        }

        public static void ConfigureLogging(this ILoggingBuilder loggingBuilder, IConfiguration configuration)
        {
            var logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();
            loggingBuilder.ClearProviders();
            loggingBuilder.AddProvider(new SerilogLoggerProvider(logger));
        }
    }
}
