using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using finam_downloader.models;

namespace finam_downloader
{
    public interface IFinamQuoteLoaderClient
    {
        Task<IEnumerable<FinamStockDTO>> StockSearch(string marketName, string code = "");

        Task<IEnumerable<FinamQuoteDTO>> Download(string marketName, int stockId, string timeFrameName,
            DateTime @from, DateTime till);
    }
}