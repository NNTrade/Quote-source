using System;
using System.Linq;
using System.Threading.Tasks;
using database;
using database.entity;
using downloader_interactor;
using Microsoft.EntityFrameworkCore;

namespace AppCore
{
    public class Initer : IIniter
    {
        private readonly QuoteSourceDbContext _dbContext;
        private readonly IDownloader _downloader;

        public Initer(QuoteSourceDbContext dbContext, IDownloader downloader)
        {
            _dbContext = dbContext;
            _downloader = downloader;
            _dbContext!.Database.Migrate();
        }

        public void CheckInitStock()
        {
            if (!_dbContext.Stock.Any())
            {
                InitStocks();
            }
        }
        public void InitStocks()
        {
            foreach (Market.Enum market in Enum.GetValues<Market.Enum>())
            {
                LoadStocks(market).Wait();
            }
            //await Parallel.ForEachAsync(Enum.GetValues<Market.Enum>(), (marketId, token) => LoadStocks(marketId));
        }

        private async Task LoadStocks(Market.Enum marketId)
        {
            var market = _dbContext.Market.Single(m => m.Id == marketId);
            var _stocks = await _downloader.StockSearch(marketId);
            foreach (Stock _stock in _stocks)
            {
                _stock.Market = market;
            }

            await _dbContext.Stock.AddRangeAsync(_stocks);
            await _dbContext.SaveChangesAsync();
        }
    }
}
