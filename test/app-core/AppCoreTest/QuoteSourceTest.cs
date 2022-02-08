using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppCore;
using AppCore.models;
using database;
using database.entity;
using database_test;
using downloader_interactor;
using NSubstitute;
using Xunit;
using Xunit.Abstractions;

namespace AppCoreTest
{
    public class QuoteSourceTest : BaseTest
    {
        public QuoteSourceTest(ITestOutputHelper output) : base(nameof(QuoteSourceTest), output)
        {
        }

        [Fact]
        public void LoadNewQuotesToDb()
        {
            #region Array

            Stock newStock;
            using (var array_dbContext = new QuoteSourceDbContext(_optionsBuilder.Options))
            {
                newStock = Stock.Build(array_dbContext, Market.Enum.UsaStock, "AAPL", "Apple", 123);
                array_dbContext.Stock.Add(newStock);
                array_dbContext.SaveChanges();
            }

            var expected_tf = TimeFrame.Enum.Daily;
            var expected_from_dt = new DateTime(2020, 1, 1);
            var expected_till_dt = new DateTime(2020, 1, 2);
            var expected_list = new List<Quote>()
            {
                new Quote()
                {
                    CandleStart = new DateTime(2020, 1, 1, 1, 1, 1),
                    Open = 123.123m,
                    High = 123.124m,
                    Low = 123.125m,
                    Close = 123.126m,
                    Volume = 1234,
                },
                new Quote()
                {
                    CandleStart = new DateTime(2020, 1, 1, 1, 1, 2),
                    Open = 124.123m,
                    High = 124.124m,
                    Low = 124.125m,
                    Close = 124.126m,
                    Volume = 123456789 , //123 456 789
                }
            };
            IDownloader _downloader = Substitute.For<IDownloader>();
            _downloader.Download(newStock.Id, expected_tf, expected_from_dt, expected_till_dt)
                .Returns((info => Task.FromResult<IEnumerable<Quote>>(expected_list)));

            #endregion

            #region Act

            IList<CandleDTO> asserted_list;
            using (var act_dbContext = new QuoteSourceDbContext(_optionsBuilder.Options))
            {
                QuoteSource _quoteSource =
                    new QuoteSource(act_dbContext, _output.BuildLoggerFor<QuoteSource>(), _downloader);
                asserted_list = _quoteSource.Download(newStock.Id, (int)TimeFrame.Enum.Daily,
                    new DateOnly(expected_from_dt.Year, expected_from_dt.Month, expected_from_dt.Day),
                    new DateOnly(expected_till_dt.Year, expected_till_dt.Month, expected_till_dt.Day)).Result;
            }

            #endregion

            #region Assert

            Assert.Equal(expected_list.Count, asserted_list.Count);
            foreach (Quote expected_quote in expected_list)
            {
                var asserted_quote = asserted_list.Single(s => s.StartDateTime == expected_quote.CandleStart);
                Assert.Equal(expected_quote.Open, asserted_quote.Open);
                Assert.Equal(expected_quote.High, asserted_quote.High);
                Assert.Equal(expected_quote.Low, asserted_quote.Low);
                Assert.Equal(expected_quote.Close, asserted_quote.Close);
                Assert.Equal(expected_quote.Volume, asserted_quote.Volume);
            }

            using (var assert_dbContext = new QuoteSourceDbContext(_optionsBuilder.Options))
            {
                var asserted_stf =
                    assert_dbContext.StockTimeFrame.Single(s => s.TimeFrameId == expected_tf && s.StockId == newStock.Id);
                Assert.Equal(expected_from_dt, asserted_stf.LoadedFrom);
                Assert.Equal(expected_till_dt, asserted_stf.LoadedTill);

                var asserted_db_quotes = assert_dbContext.Quote.Where(q => q.StockTimeFrameId == asserted_stf.Id).ToList();
                Assert.Equal(expected_list.Count, asserted_db_quotes.Count);
                foreach (Quote expected_quote in expected_list)
                {
                    var asserted_db_quote = asserted_db_quotes.Single(s => s.CandleStart == expected_quote.CandleStart);
                    Assert.Equal(expected_quote.Open, asserted_db_quote.Open);
                    Assert.Equal(expected_quote.High, asserted_db_quote.High);
                    Assert.Equal(expected_quote.Low, asserted_db_quote.Low);
                    Assert.Equal(expected_quote.Close, asserted_db_quote.Close);
                    Assert.Equal(expected_quote.Volume, asserted_db_quote.Volume);
                }
            }

            #endregion
        }
    }
}
