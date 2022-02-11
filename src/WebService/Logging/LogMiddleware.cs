using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace WebService.Logging
{
    public static class LogMiddlewareExtension
    {
        public static void AddCallLog(this WebApplication webApplication)
        {
            webApplication.Use(async (context, next) =>
            {
                context.RequestServices.GetService<ILoggerProvider>()
                    ?.CreateLogger("Controllers")
                    .LogInformation("Called url {$url}", context.Request.Path);
                await next(context);
            });
        }
    }
}
