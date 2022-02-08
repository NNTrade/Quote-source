using System;
using System.Linq;
using System.Threading.Tasks;
using database;
using database.entity;
using migration;
using Xunit;
using Xunit.Abstractions;

namespace database_test
{
    public class AddDateTimeTest : BaseTest
    {
        private Stock newStock;

        public AddDateTimeTest(ITestOutputHelper output) : base(
            nameof(QuoteSourceDbContext) + "_" + nameof(AddDateTimeTest), output)
        {
            using var dbContext = new ContextFactory().CreateDbContext();
            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();

            newStock = new Stock()
            {
                Code = "EURUSD",
                FinamId = 10,
                Market = dbContext.Market.Single(m => m.Id == Market.Enum.Forex),
                MarketId = Market.Enum.Forex,
                Name = "EUR vs USD"
            };

            dbContext.Stock.Add(newStock);
            dbContext.SaveChanges();
        }

        [Fact]
        public async Task AddDataToDbSetTest()
        {
            StockTimeFrame expected_stf;
            using (var dbContext = new ContextFactory().CreateDbContext())
            {
                #region Array

                expected_stf = new StockTimeFrame()
                {
                    StockId = newStock.Id,
                    Stock = dbContext.Stock.Single(s => s.Id == newStock.Id),
                    TimeFrameId = TimeFrame.Enum.Daily,
                    TimeFrame = dbContext.TimeFrame.Single(tf => tf.Id == TimeFrame.Enum.Daily),
                    LoadedFrom = new DateTime(2020,1,2,0,0,0,DateTimeKind.Utc),
                    LoadedTill = new DateTime(2020,1,3,0,0,0,DateTimeKind.Utc)
                };
                #endregion

                #region Act

                dbContext.StockTimeFrame.Add(expected_stf);
                dbContext.SaveChanges();

                #endregion
            }

            using var dbContext2 = new ContextFactory().CreateDbContext();
            #region Assert

            var asserted_stf = dbContext2.StockTimeFrame.Single(s => s.Id == expected_stf.Id);
            _output.WriteLine($"Expected from {expected_stf.LoadedFrom.ToString()}");
            _output.WriteLine($"Asserted from {asserted_stf.LoadedFrom.ToString()}");
            Assert.Equal(expected_stf.LoadedFrom, asserted_stf.LoadedFrom);
            Assert.Equal(expected_stf.LoadedTill, asserted_stf.LoadedTill);


            #endregion
        }
    }
}
