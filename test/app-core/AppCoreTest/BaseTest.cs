using System;
using System.Data.Common;
using database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Npgsql;
using Xunit.Abstractions;

namespace database_test
{
    public abstract class BaseTest: IDisposable
    {
        protected readonly ITestOutputHelper _output;
        protected DbContextOptionsBuilder _optionsBuilder;

        private static DbContextOptionsBuilder GetOptionBuilder(string dbSuffix)
        {
            var _configurationBuilder = new ConfigurationBuilder();
            _configurationBuilder.AddJsonFile("appsettings.json");

            var _config = _configurationBuilder.Build();

            DbConnectionStringBuilder _connectionStringBuilder =
                new NpgsqlConnectionStringBuilder(_config.GetConnectionString("DefaultConnection"));
            _connectionStringBuilder["Database"] += dbSuffix;

            var _optionsBuilder = new DbContextOptionsBuilder();

            _optionsBuilder.UseNpgsql(_connectionStringBuilder.ConnectionString);
            return _optionsBuilder;
        }

        public BaseTest(string dbSuffix,ITestOutputHelper output)
        {
            _output = output;
            _optionsBuilder = GetOptionBuilder(dbSuffix);
            using (var _context = new QuoteSourceDbContext(_optionsBuilder.Options))
            {
                _context.Database.EnsureDeleted();
                _context.Database.Migrate();
            }
        }

        public void Dispose()
        {
            using (var _context = new QuoteSourceDbContext(_optionsBuilder.Options))
            {
                _context.Database.EnsureDeleted();
            }
        }
    }
}
