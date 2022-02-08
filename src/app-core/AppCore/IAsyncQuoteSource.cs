using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AppCore.models;

namespace AppCore
{
    public interface IAsyncQuoteSource
    {
        public Task<IList<CandleDTO>> Download(int stockId, int timeFrameId, DateOnly @from, DateOnly till);

        public Task<IList<StockDTO>> StockSearch(int marketId, string code);
        public Task<List<MarketDTO>> Markets { get; }
        public Task<List<TimeFrameDTO>> TimeFrames { get; }
        public TimeFrameDTO GetTimeFrame(string name);
    }
}
