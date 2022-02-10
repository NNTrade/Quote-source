using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace WebService
{
    public static class RoutingMap
    {
        private static void RedirectToSwagger(HttpContext context)
        {
            context.Response.Redirect("/swagger/index.html");
        }

        public static void AddRedirectToSwagger(this WebApplication webApplication)
        {
            webApplication.Map("/", RedirectToSwagger);
        }
    }
}
