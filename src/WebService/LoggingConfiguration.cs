using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Extensions.Logging;

namespace WebService
{
    public static class LoggingConfiguration
    {
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
