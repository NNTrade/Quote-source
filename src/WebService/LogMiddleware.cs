using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace WebService
{
    public static class LogMiddlewareExtension
    {
        public static void AddCallLog(this WebApplication webApplication)
        {
            webApplication.Use(async (context, next) =>
            {
                context.RequestServices.GetService<ILogger>().Information("Called url ${url}", context.Request.Path);
                await next(context);
            });
        }
    }
}
