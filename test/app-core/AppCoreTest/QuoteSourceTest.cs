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
using finam_downloader;
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
        public async Task ExcludedStocksDoesntFiltered()
        {
            #region Array

            Stock _excludedStock;
            Stock _stock;
            using (var _arrayDBContext = new QuoteSourceDbContext(_optionsBuilder.Options))
            {
                _excludedStock = Stock.Build(_arrayDBContext, Market.Enum.UsaStock, "AAPL", "exApple", 123);
                _arrayDBContext.Stock.Add(_excludedStock);

                _stock = Stock.Build(_arrayDBContext, Market.Enum.UsaStock, "AAPL", "Apple", 124);
                _arrayDBContext.Stock.Add(_stock);
                _arrayDBContext.SaveChanges();

                await new ExcludeFilter(_arrayDBContext, _output.BuildLoggerFor<ExcludeFilter>()).Add(_excludedStock.Id);
            }

            IDownloader _downloader = Substitute.For<IDownloader>();

            #endregion

            #region Act

            IList<StockDTO> _assertedList;
            using (var _actDBContext = new QuoteSourceDbContext(_optionsBuilder.Options))
            {
                QuoteSource _quoteSource =
                    new QuoteSource(_actDBContext, _output.BuildLoggerFor<QuoteSource>(), _downloader);

                _assertedList = await _quoteSource.StockSearch((int)Market.Enum.UsaStock, "AAPL");
            }

            #endregion

            #region Assert

            Assert.Equal(1, _assertedList.Count);
            Assert.Equal(_stock.Id, _assertedList[0].Id);

            #endregion
        }

        [Fact]
        public async Task ExcludedStocksDoesntLoad()
        {
            #region Array

            Stock _newStock;
            using (var _arrayDBContext = new QuoteSourceDbContext(_optionsBuilder.Options))
            {
                _newStock = Stock.Build(_arrayDBContext, Market.Enum.UsaStock, "AAPL", "Apple", 123);
                _arrayDBContext.Stock.Add(_newStock);
                _arrayDBContext.SaveChanges();

                await new ExcludeFilter(_arrayDBContext, _output.BuildLoggerFor<ExcludeFilter>()).Add(_newStock.Id);
            }

            IDownloader _downloader = Substitute.For<IDownloader>();

            #endregion

            #region Assert

            IList<CandleDTO> _assertedList;
            using (var _assertDBContext = new QuoteSourceDbContext(_optionsBuilder.Options))
            {
                QuoteSource _quoteSource =
                    new QuoteSource(_assertDBContext, _output.BuildLoggerFor<QuoteSource>(), _downloader);

                await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => _quoteSource.Download(_newStock.Id,
                    (int)TimeFrame.Enum.Day, DateOnly.FromDateTime(DateTime.Now),
                    DateOnly.FromDateTime(DateTime.Now)));
            }

            #endregion
        }

        [Fact]
        public void LoadNewQuotesToDb()
        {
            #region Array

            Stock _newStock;
            using (var _arrayDBContext = new QuoteSourceDbContext(_optionsBuilder.Options))
            {
                _newStock = Stock.Build(_arrayDBContext, Market.Enum.UsaStock, "AAPL", "Apple", 123);
                _arrayDBContext.Stock.Add(_newStock);
                _arrayDBContext.SaveChanges();
            }

            var _expectedTf = TimeFrame.Enum.Day;
            var _expectedFromDt = new DateTime(2020, 1, 1);
            var _expectedTillDt = new DateTime(2020, 1, 3) - TimeSpan.FromMilliseconds(1);
            var _expectedList = new List<Quote>()
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
                    Volume = 123456789, //123 456 789
                }
            };
            IDownloader _downloader = Substitute.For<IDownloader>();
            _downloader.Download(_newStock.Id, _expectedTf, _expectedFromDt, _expectedTillDt)
                .Returns((info => Task.FromResult<IEnumerable<Quote>>(_expectedList)));

            #endregion

            #region Act

            IList<CandleDTO> _assertedList;
            using (var _actDBContext = new QuoteSourceDbContext(_optionsBuilder.Options))
            {
                QuoteSource _quoteSource =
                    new QuoteSource(_actDBContext, _output.BuildLoggerFor<QuoteSource>(), _downloader);
                _assertedList = _quoteSource.Download(_newStock.Id, (int)TimeFrame.Enum.Day,
                    new DateOnly(_expectedFromDt.Year, _expectedFromDt.Month, _expectedFromDt.Day),
                    new DateOnly(_expectedTillDt.Year, _expectedTillDt.Month, _expectedTillDt.Day)).Result;
            }

            #endregion

            #region Assert

            Assert.Equal(_expectedList.Count, _assertedList.Count);
            foreach (Quote _expectedQuote in _expectedList)
            {
                var _assertedQuote = _assertedList.Single(s => s.StartDateTime == _expectedQuote.CandleStart);
                Assert.Equal(_expectedQuote.Open, _assertedQuote.Open);
                Assert.Equal(_expectedQuote.High, _assertedQuote.High);
                Assert.Equal(_expectedQuote.Low, _assertedQuote.Low);
                Assert.Equal(_expectedQuote.Close, _assertedQuote.Close);
                Assert.Equal(_expectedQuote.Volume, _assertedQuote.Volume);
            }

            using (var _assertDBContext = new QuoteSourceDbContext(_optionsBuilder.Options))
            {
                var _assertedStf =
                    _assertDBContext.StockTimeFrame.Single(
                        s => s.TimeFrameId == _expectedTf && s.StockId == _newStock.Id);
                Assert.Equal(_expectedFromDt, _assertedStf.LoadedFrom);
                Assert.Equal(_expectedTillDt, _assertedStf.LoadedTill);

                var _assertedDBQuotes =
                    _assertDBContext.Quote.Where(q => q.StockTimeFrameId == _assertedStf.Id).ToList();
                Assert.Equal(_expectedList.Count, _assertedDBQuotes.Count);
                foreach (Quote _expectedQuote in _expectedList)
                {
                    var _assertedDBQuote =
                        _assertedDBQuotes.Single(s => s.CandleStart == _expectedQuote.CandleStart);
                    Assert.Equal(_expectedQuote.Open, _assertedDBQuote.Open);
                    Assert.Equal(_expectedQuote.High, _assertedDBQuote.High);
                    Assert.Equal(_expectedQuote.Low, _assertedDBQuote.Low);
                    Assert.Equal(_expectedQuote.Close, _assertedDBQuote.Close);
                    Assert.Equal(_expectedQuote.Volume, _assertedDBQuote.Volume);
                }
            }

            #endregion
        }
    }
}
