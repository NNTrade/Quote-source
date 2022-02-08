using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using database.entity;
namespace downloader_interactor
{
    public interface IDownloader
    {
        public Task<IEnumerable<Quote>> Download(int stockId, TimeFrame.Enum timeFrameId, DateTime @from, DateTime till);

        public Task<IEnumerable<Stock>> StockSearch(Market.Enum marketId, string code = "");
    }
}
