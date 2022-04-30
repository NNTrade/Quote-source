using System;
using System.Linq;
using System.Threading.Tasks;
using database;
using database.entity;
using downloader_interactor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AppCore
{
    public class Initer : IIniter
    {
        private readonly QuoteSourceDbContext _dbContext;
        private readonly IDownloader _downloader;
        private readonly ILogger<IIniter> _logger;
        private readonly IExcludeFilter _excludeFilter;

        public Initer(QuoteSourceDbContext dbContext, IDownloader downloader, ILogger<IIniter> logger, IExcludeFilter excludeFilter)
        {
            _dbContext = dbContext;
            _downloader = downloader;
            _logger = logger;
            _excludeFilter = excludeFilter;
        }

        public void Reinit()
        {
            _logger.LogInformation($"Reinit database. Connection string: {_dbContext.Database.GetConnectionString()}");
            _dbContext!.Database.EnsureDeleted();
            _dbContext!.Database.Migrate();
            InitStocks();
            _logger.LogInformation($"Reinit database completed");
        }

        public void CheckInitStock()
        {
            _logger.LogInformation(
                $"Migrate to data base. Connection string: {_dbContext.Database.GetConnectionString()}");
            _dbContext!.Database.Migrate();
            if (!_dbContext.Stock.Any())
            {
                _logger.LogInformation("Stock list is Empty. Load stocks");
                InitStocks();
            }
        }

        public void InitStocks()
        {
            foreach (Market.Enum market in Enum.GetValues<Market.Enum>())
            {
                _logger.LogInformation($"Load stock from market: {market.ToString()}");
                LoadStocks(market).Wait();
            }
            ExcludeBase().Wait();
            //await Parallel.ForEachAsync(Enum.GetValues<Market.Enum>(), (marketId, token) => LoadStocks(marketId));
        }

        private async Task ExcludeBase()
        {
            var excludedBTC_USD = _dbContext.Stock.Single(s => s.FinamId == 499054);
            await _excludeFilter.Add(excludedBTC_USD.Id);
        }

        private async Task LoadStocks(Market.Enum marketId)
        {
            var market = _dbContext.Market.Single(m => m.Id == marketId);
            var _stocks = await _downloader.StockSearch(marketId);
            foreach (Stock _stock in _stocks)
            {
                _stock.MarketId = marketId;
                _stock.Market = market;
            }

            var _duplicatedStocks = _stocks.GroupBy(s => s.Code).Where(s => s.Count() > 1);
            if (_duplicatedStocks.Any())
            {
                foreach (var _duplicatedStock in _duplicatedStocks)
                {
                    _logger.LogWarning("Stock with code {@Code} has duplicates {@times} times", _duplicatedStock.Key,
                        _duplicatedStock.Count());
                }
            }

            await _dbContext.Stock.AddRangeAsync(_stocks);
            await _dbContext.SaveChangesAsync();
            _logger.LogInformation($"Load {_stocks.Count()} for market {marketId.ToString()}");
        }
    }
}
