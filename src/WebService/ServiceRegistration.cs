using System;
using System.Reflection;
using database;
using downloader_interactor;
using finam_downloader;
using AppCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Sinks.Elasticsearch;

namespace WebService
{
    public static class ServiceRegistration
    {
        public static ILogger<T> GetLogger<T>(this IServiceProvider serviceProvider)
        {
            return serviceProvider.GetService<ILoggerFactory>()!.CreateLogger<T>();
        }


        public static IServiceCollection AddDatabase(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddDbContext<QuoteSourceDbContext>((provider, builder) =>
            {
                builder.UseNpgsql(provider.GetService<IConfiguration>()
                    .GetConnectionString("DefaultConnection"));
                builder.EnableDetailedErrors();
            });
            return serviceCollection;
        }

        public static IServiceCollection AddServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddHttpClient<IFinamQuoteLoaderClient, FinamQuoteLoaderClient>((client, provider) =>
            {
                var _finamUri = new Uri(provider.GetService<IConfiguration>()!["FinamLoaderUrl"]);
                client.BaseAddress = _finamUri;
                return new FinamQuoteLoaderClient(provider.GetLogger<FinamQuoteLoaderClient>(), client);
            });
            serviceCollection.AddScoped<IDownloader, FinamDownloader>();
            serviceCollection.AddScoped<IAsyncQuoteSource, QuoteSource>();
            serviceCollection.AddScoped<IExcludeFilter, ExcludeFilter>();
            serviceCollection.AddScoped<IIniter, Initer>();

            return serviceCollection;
        }

        public static void DropDb(this IServiceProvider serviceProvider)
        {
            using var _scope = serviceProvider.CreateScope();
            var _sp = _scope.ServiceProvider;
            _sp.GetService<QuoteSourceDbContext>()?.Database.EnsureDeleted();
        }

        public static void CheckDbInit(this IServiceProvider serviceProvider)
        {
            using var _scope = serviceProvider.CreateScope();
            var _sp = _scope.ServiceProvider;
            _sp.GetService<IIniter>()!.CheckInitStock();
        }
    }
}
