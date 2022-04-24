using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppCore.models;
using database;
using database.entity;
using downloader_interactor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MoreLinq.Extensions;

namespace AppCore
{
    public class QuoteSource : IAsyncQuoteSource
    {
        private readonly QuoteSourceDbContext _dbContext;
        private readonly ILogger<QuoteSource> _logger;
        private readonly IDownloader _downloader;

        public QuoteSource(QuoteSourceDbContext dbContext, ILogger<QuoteSource> logger, IDownloader downloader)
        {
            _dbContext = dbContext;
            _logger = logger;
            _downloader = downloader;
        }

        public async Task<IList<CandleDTO>> Download(int stockId, int timeFrameId, DateOnly @from, DateOnly till)
        {
            var from_dt = new DateTime(@from.Year, @from.Month, @from.Day, 0, 0, 0, DateTimeKind.Utc);
            var till_dt = new DateTime(@till.Year, @till.Month, @till.Day, 0, 0, 0, DateTimeKind.Utc);
            till_dt = till_dt + TimeSpan.FromDays(1) - TimeSpan.FromMilliseconds(1);

            var _stockTimeFrames = _dbContext.StockTimeFrame
                .Include(s => s.Stock)
                .Include(s => s.TimeFrame)
                .SingleOrDefault(s => s.StockId == stockId && s.TimeFrameId == (TimeFrame.Enum)timeFrameId);

            if (_stockTimeFrames == null)
            {
                var stock = await _dbContext.Stock.SingleOrDefaultAsync(s => s.Id == stockId);
                if (stock == null)
                {
                    _logger.LogException(LogEvent.NoStock,
                        new ArgumentOutOfRangeException(nameof(stockId), stockId,
                            $"Couldn't find stock"),
                        "Try load unknown stock {@stock}", stockId);
                }

                var tf = await _dbContext.TimeFrame.SingleOrDefaultAsync(t => t.Id == (TimeFrame.Enum)timeFrameId);
                if (tf == null)
                {
                    _logger.LogException(LogEvent.NoStock,
                        new ArgumentOutOfRangeException(nameof(timeFrameId), timeFrameId,
                            $"Couldn't find timeframe"),
                        "Try load unknown stock {@stock}", stockId);
                }

                _logger.LogInformation(LogEvent.NoStockTimeFrame.GetEventId(),
                    "Request new TimeFrame {@timeframe} for stock {@stock}", (tf.Name), stock.Code);

                _stockTimeFrames = await LoadAll(stockId, (TimeFrame.Enum)timeFrameId, @from_dt, @till_dt);
            }

            if (from_dt < _stockTimeFrames.LoadedFrom)
            {
                await DownloadFrom(_stockTimeFrames, @from_dt);
            }

            if (@till_dt > _stockTimeFrames.LoadedTill)
            {
                await DownloadTill(_stockTimeFrames, @till_dt);
            }

            return await _dbContext.Quote.Where(q =>
                    q.StockTimeFrameId == _stockTimeFrames.Id && q.CandleStart >= from_dt && q.CandleStart <= till_dt)
                .Select(q => q.ToDTO()).ToListAsync();
        }

        private async Task DownloadFrom(StockTimeFrame stockTimeFrame, DateTime @from)
        {
            stockTimeFrame.LoadedFrom = @from;
            await LoadStockTimeFrame(stockTimeFrame, @from, stockTimeFrame.LoadedFrom);
        }

        private async Task DownloadTill(StockTimeFrame stockTimeFrame, DateTime @till)
        {
            stockTimeFrame.LoadedTill = @till;
            await LoadStockTimeFrame(stockTimeFrame, stockTimeFrame.LoadedTill, till);
        }

        private async Task<StockTimeFrame> LoadAll(int stockId, TimeFrame.Enum timeFrameId, DateTime @from,
            DateTime @till)
        {
            StockTimeFrame stockTimeFrame = new()
            {
                StockId = stockId,
                TimeFrameId = (TimeFrame.Enum)timeFrameId,
                LoadedFrom = @from,
                LoadedTill = @till,
                Stock = _dbContext.Stock.Single(s => s.Id == stockId),
                TimeFrame = _dbContext.TimeFrame.Single(s => s.Id == timeFrameId)
            };

            await LoadStockTimeFrame(stockTimeFrame, @from, till);

            return stockTimeFrame;
        }

        private async Task LoadStockTimeFrame(StockTimeFrame stockTimeFrame, DateTime @from, DateTime till)
        {
            using var _transaction = _dbContext.Database.BeginTransaction();
            _logger.LogInformation(LogEvent.LoadData.GetEventId(),
                "Start loading data for stock {@stock} timeframe {@tf} interval {@from} - {@till}", stockTimeFrame
                    .Stock
                    .Code, stockTimeFrame.TimeFrame.Name, from, till);

            _dbContext.StockTimeFrame.Update(stockTimeFrame);
            await _dbContext.SaveChangesAsync();

            var newFrom = from;
            var newTill = from;

            do
            {
                newTill += new TimeSpan(365, 0, 0, 0);
                if (newTill >= till) newTill = till;

                await LoadStocks(stockTimeFrame, newFrom, newTill);
                newFrom = newTill;
            } while (newFrom < till);

            await _transaction.CommitAsync();
            _logger.LogInformation(LogEvent.LoadData.GetEventId(), "Loading new data for Stock TimeFrame completed");
        }

        private async Task LoadStocks(StockTimeFrame _stockTimeFrame, DateTime @from, DateTime till)
        {
            _logger.LogInformation(LogEvent.LoadData.GetEventId(),
                "Downloading data: interval {@from} - {@till}", from, till);
            var _newStocks =
                (await _downloader.Download(_stockTimeFrame.StockId, _stockTimeFrame.TimeFrameId, @from, @till))
                .ToArray();
            _logger.LogInformation(LogEvent.LoadData.GetEventId(), "Download success, load {@rows} rows",
                _newStocks.Length);

            try
            {
                _newStocks.ForEach(quote =>
                {
                    quote.CandleStart = new DateTime(quote.CandleStart.Year, quote.CandleStart.Month,
                        quote.CandleStart.Day,
                        quote.CandleStart.Hour, quote.CandleStart.Minute, quote.CandleStart.Second, DateTimeKind.Utc);
                    quote.StockTimeFrameId = _stockTimeFrame.Id;
                    quote.StockTimeFrame = _stockTimeFrame;
                });

                var _uniqueNewStocks = RemoveDuplicateQuote(_stockTimeFrame, _newStocks).ToArray();
                _logger.LogInformation(LogEvent.LoadData.GetEventId(), "Remove {@dup} duplicates while compare with DB",
                    (_newStocks.Length - _uniqueNewStocks.Length));

                _logger.LogInformation(LogEvent.LoadData.GetEventId(), "Append new {@rows} rows to DB",
                    _uniqueNewStocks.Length);
                await _dbContext.Quote.AddRangeAsync(_uniqueNewStocks);
                await _dbContext.SaveChangesAsync();
                _logger.LogInformation(LogEvent.LoadData.GetEventId(), "New data saved");
            }
            catch (Exception e)
            {
                _logger.LogCritical(e.Message);
                throw new ApplicationException("Errors while save data to database");
            }
        }

        private IEnumerable<Quote> RemoveDuplicateQuote(StockTimeFrame stockTimeFrame, IEnumerable<Quote> newStocks)
        {
            var newDT = newStocks.Select(ns => ns.CandleStart).ToArray();
            var dupDT = _dbContext.Quote
                .Where(q => q.StockTimeFrameId == stockTimeFrame.Id && newDT.Contains(q.CandleStart))
                .Select(q => q.CandleStart).ToArray();

            return dupDT.Length == 0 ? newStocks : newStocks.Where(ns => !dupDT.Contains(ns.CandleStart));
        }

        public async Task<IList<StockDTO>> StockSearch(int marketId, string code)
        {
            var _stocks = _dbContext.Stock.Where(s => s.MarketId == (Market.Enum)marketId && s.Code.Contains(code));
            return await _stocks.Select(s => s.ToDto()).ToListAsync();
        }

        public Task<List<MarketDTO>> Markets => _dbContext.Market.Select(m => m.ToDto()).ToListAsync();


        public Task<List<TimeFrameDTO>> TimeFrames => _dbContext.TimeFrame.Select(t => t.ToDto()).ToListAsync();

        public TimeFrameDTO GetTimeFrame(string name)
        {
            var tfEnt = _dbContext.TimeFrame.SingleOrDefault(t => t.Name == name);
            if (tfEnt == null)
            {
                throw new ArgumentOutOfRangeException(nameof(name), name, $"There is no time frame {name}");
            }

            return tfEnt.ToDto();
        }

        public Task<List<StockDTO>> TopStock(int stockId, int yearsOfCount)
        {
            throw new NotImplementedException();
        }
    }
}
