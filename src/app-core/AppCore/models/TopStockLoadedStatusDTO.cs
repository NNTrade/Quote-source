using System.Collections.Generic;

namespace AppCore.models
{
    public class TopStockLoadedStatusDTO
    {
        public List<StockDTO> TopLoadedStocks { get; set; }
        public List<StockDTO> UnloadedStocks { get; set; }
    }
}
