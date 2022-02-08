using database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace migration
{
    public class ContextFactory : IDesignTimeDbContextFactory<QuoteSourceDbContext>
    {
        private IConfigurationBuilder _configurationBuilder;

        public ContextFactory()
        {
            _configurationBuilder = new ConfigurationBuilder();
            _configurationBuilder.AddJsonFile("appsettings.json");
        }

        public QuoteSourceDbContext CreateDbContext(string[] args)
        {
            return CreateDbContext();
        }

        public QuoteSourceDbContext CreateDbContext()
        {
            var _config = _configurationBuilder.Build();
            string connectionString = _config.GetConnectionString("DefaultConnection");

            var optionsBuilder = new DbContextOptionsBuilder<QuoteSourceDbContext>();
            optionsBuilder.UseNpgsql(connectionString);

            return new QuoteSourceDbContext(optionsBuilder.Options);
        }
    }
}
