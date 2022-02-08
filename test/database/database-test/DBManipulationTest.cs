using System.Linq;
using System.Threading.Tasks;
using database;
using database.entity;
using migration;
using Xunit;
using Xunit.Abstractions;

namespace database_test
{
    public class DBManipulationTest : BaseTest
    {
        public DBManipulationTest(ITestOutputHelper output) : base(
            nameof(QuoteSourceDbContext) + "_" + nameof(DBManipulationTest), output)
        {
        }

        [Fact]
        public async Task AddDataToDbSetTest()
        {
            #region Array

            using var dbContext = new ContextFactory().CreateDbContext();
            await dbContext.Database.EnsureDeletedAsync();
            await dbContext.Database.EnsureCreatedAsync();

            #endregion

            #region Act
            _output.WriteLine("Только если заполнить Market и MarketId строка добавится");
            var newStock = new Stock()
            {
                Code = "EURUSD",
                FinamId = 10,
                Market = dbContext.Market.Single(m=>m.Id == Market.Enum.Forex),
                MarketId = Market.Enum.Forex,
                Name = "EUR vs USD"
            };

            dbContext.Stock.Add(newStock);
            dbContext.SaveChanges();

            #endregion

            #region Assert

            #endregion
        }

        [Fact]
        public async Task AddDataToParentDbSetTest()
        {
            #region Array

            using var dbContext = new ContextFactory().CreateDbContext();
            await dbContext.Database.EnsureDeletedAsync();
            await dbContext.Database.EnsureCreatedAsync();

            #endregion

            #region Act

            var market = dbContext.Market.Single(m => m.Id == Market.Enum.Forex);
            market.Stocks.Add(new Stock()
            {
                Code = "EURUSD",
                FinamId = 10,
                MarketId = market.Id,
                Market = market,
                Name = "EUR vs USD"
            });
            dbContext.SaveChanges();

            #endregion

            #region Assert

            #endregion
        }
    }
}
