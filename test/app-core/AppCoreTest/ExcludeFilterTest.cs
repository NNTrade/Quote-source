using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppCore;
using AppCore.models;
using database;
using database.entity;
using database_test;
using Xunit;
using Xunit.Abstractions;

namespace AppCoreTest
{
    public class DuplicateFilterTest: BaseTest
    {
        public DuplicateFilterTest(ITestOutputHelper output) : base(nameof(DuplicateFilterTest), output)
        {
        }

        [Fact]
        public async Task AddExcludeToDb()
        {
            #region Array
            Stock test_stock;
            using (var array_dbContext = CreateContext())
            {
                test_stock = Stock.Build(array_dbContext,Market.Enum.MmvbStock, "USDTEST", "Test stock", 123);
                array_dbContext.Stock.Add(test_stock);
                await array_dbContext.SaveChangesAsync();
            }

            #endregion

            #region Act

            using (var act_dbContext = CreateContext())
            {
                var filterTool = new ExcludeFilter(act_dbContext, _output.BuildLoggerFor<ExcludeFilter>());
                await filterTool.Add(test_stock.Id);
            }

            #endregion

            #region Assert

            using (var assert_dbContext = CreateContext())
            {
                var assertStock = assert_dbContext.Stock.Single(s => s.Id == test_stock.Id);
                Assert.True(assertStock.Exclude);
            }

            #endregion
        }

        [Fact]
        public async Task RemoveExcludeToDb()
        {
            #region Array
            Stock test_stock;
            using (var array_dbContext = CreateContext())
            {
                test_stock = Stock.Build(array_dbContext,Market.Enum.MmvbStock, "USDTEST", "Test stock", 123);
                array_dbContext.Stock.Add(test_stock);
                await array_dbContext.SaveChangesAsync();

                var filterTool = new ExcludeFilter(array_dbContext, _output.BuildLoggerFor<ExcludeFilter>());
                await filterTool.Add(test_stock.Id);
            }

            #endregion

            #region Act

            using (var act_dbContext = CreateContext())
            {
                var filterTool = new ExcludeFilter(act_dbContext, _output.BuildLoggerFor<ExcludeFilter>());
                await filterTool.Remove(test_stock.Id);
            }

            #endregion

            #region Assert

            using (var assert_dbContext = CreateContext())
            {
                var assertStock = assert_dbContext.Stock.Single(s => s.Id == test_stock.Id);
                Assert.False(assertStock.Exclude);
            }

            #endregion
        }

        [Fact]
        public async Task GetListOfExcludeStock()
        {
            #region Array
            Stock test_stock;
            Stock test_stock_non_exclude;
            using (var array_dbContext = CreateContext())
            {
                test_stock = Stock.Build(array_dbContext,Market.Enum.MmvbStock, "USDTEST", "Test stock", 123);
                array_dbContext.Stock.Add(test_stock);
                test_stock_non_exclude = Stock.Build(array_dbContext,Market.Enum.MmvbStock, "USDTEST2", "Test stock2", 124);
                array_dbContext.Stock.Add(test_stock_non_exclude);

                await array_dbContext.SaveChangesAsync();

                var filterTool = new ExcludeFilter(array_dbContext, _output.BuildLoggerFor<ExcludeFilter>());
                await filterTool.Add(test_stock.Id);
            }

            #endregion

            #region Act

            List<StockDTO> asserted_Stocks;
            using (var act_dbContext = CreateContext())
            {
                var filterTool = new ExcludeFilter(act_dbContext, _output.BuildLoggerFor<ExcludeFilter>());
                asserted_Stocks = await filterTool.GetExcludes();
            }

            #endregion

            #region Assert

            Assert.Equal(1,asserted_Stocks.Count);
            Assert.Equal(test_stock.Id,asserted_Stocks[0].Id);

            #endregion
        }
    }
}
