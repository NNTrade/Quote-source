using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using database;
using database.entity;
using finam_downloader.models;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using IDownloader = downloader_interactor.IDownloader;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace finam_downloader
{
    public class FinamDownloader : IDownloader
    {

        private readonly QuoteSourceDbContext _dbContext;
        private readonly IFinamQuoteLoaderClient _finamQuoteLoaderClient;
        private readonly ILogger<FinamDownloader> _logger;

        public FinamDownloader(QuoteSourceDbContext dbContext,
            IFinamQuoteLoaderClient finamQuoteLoaderClient,
            ILogger<FinamDownloader> logger)
        {
            _dbContext = dbContext;
            _finamQuoteLoaderClient = finamQuoteLoaderClient;
            _logger = logger;
        }

        public async Task<IEnumerable<Stock>> StockSearch(Market.Enum marketId, string code = "")
        {
            var _market = _dbContext.Market.SingleOrDefault(m => m.Id == (Market.Enum)marketId);
            if (_market == null)
                throw new ArgumentOutOfRangeException(nameof(marketId), marketId,
                    $"There is no market with id {marketId}");
            var finam_market_name = mapper.ToFinamMarket(_market.Id);

            var _finamStocks = await _finamQuoteLoaderClient.StockSearch(finam_market_name, code);
            return _finamStocks.ToList().ConvertAll(fs => new Stock()
            {
                FinamId = fs.Id,
                MarketId = fs.Market.ParseMarket(),
                Code = fs.Code,
                Name = fs.Name
            });
        }


        public async Task<IEnumerable<Quote>> Download(int stockId, TimeFrame.Enum timeFrameId, DateTime @from, DateTime till)
        {
            var _stock = _dbContext.Stock.SingleOrDefault(s => s.Id == stockId);
            if (_stock == null)
            {
                _logger.LogError($"Try to load wrong stock Id {stockId}");
                throw new ArgumentOutOfRangeException(nameof(stockId), stockId,
                    $"Wrong stock Id {stockId}");
            }

            var _tf = _dbContext.TimeFrame.SingleOrDefault(t => t.Id == timeFrameId);
            if (_tf == null)
            {
                _logger.LogError($"Try to load wrong timeframe Id {timeFrameId}");
                throw new ArgumentOutOfRangeException(nameof(timeFrameId), timeFrameId,
                    $"Wrong timeframe Id {timeFrameId}");
            }

            if (@till < @from)
            {
                _logger.LogError($"Try to load when From Date {@from} > Till Date {@till}");
                throw new ArgumentOutOfRangeException($"Cannot load data if From Date {@from} > Till Date {@till}");
            }

            var _finamQuotes = await _finamQuoteLoaderClient.Download(_stock.MarketId.ToFinamMarket(), _stock.FinamId,
                _tf.Id.ToFinamTimeFrame(), @from, @till);
            return _finamQuotes.ToList().ConvertAll(fs => fs.ToEntity());
        }
    }
}
