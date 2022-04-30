using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AppCore;
using AppCore.models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using LogEvent = WebService.Logging.LogEvent;

namespace WebService.Controllers
{
    [ApiController]
    [Route("/api/market/exclude/stock")]
    public class StockExcludeController: ControllerBase
    {
        private readonly ILogger<StockExcludeController> _logger;
        private readonly IExcludeFilter _excludeFilter;


        public StockExcludeController(ILogger<StockExcludeController> logger, IExcludeFilter excludeFilter)
        {
            _logger = logger;
            _excludeFilter = excludeFilter;
        }

        // GET /api/market/exclude/stock?id=
        [HttpGet(Name = "Get Stocks exclude stocks")]
        [ProducesResponseType(typeof(IList<StockDTO>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetExcludedStocks()
        {
            _logger.LogInformation( LogEvent.CallEndpoint(nameof(StockExcludeController),nameof(GetExcludedStocks)),
                "Request excluded stocks");
            var _stockDtos = await _excludeFilter.GetExcludes();

            _logger.LogInformation( LogEvent.CallEndpoint(nameof(StockExcludeController),nameof(GetExcludedStocks)),
                $"Response list of excluded stocks with {_stockDtos.Count} rows");
            return Ok(_stockDtos);
        }

        // POST /api/market/exclude/stock
        [HttpPost(Name = "Add Stock to exclude stocks")]
        public async Task<IActionResult> AddStockToExclude([FromQuery]int stockId)
        {
            _logger.LogInformation( LogEvent.CallEndpoint(nameof(StockExcludeController),nameof(AddStockToExclude)),
                "Add stock to exclude list: StockId {@stockId}", stockId);
            await _excludeFilter.Add(stockId);

            _logger.LogInformation( LogEvent.CallEndpoint(nameof(StockExcludeController),nameof(AddStockToExclude)),
                $"Stock with id {@stockId} excluded", stockId);
            return Ok();
        }

        // DELETE /api/market/exclude/stock
        [HttpDelete(Name = "Remove Stock from exclude stocks")]
        public async Task<IActionResult> RemoveStockToExclude([FromQuery]int stockId)
        {
            _logger.LogInformation( LogEvent.CallEndpoint(nameof(StockExcludeController),nameof(RemoveStockToExclude)),
                "Remove stock from exclude list: StockId {@stockId}", stockId);
            await _excludeFilter.Remove(stockId);

            _logger.LogInformation( LogEvent.CallEndpoint(nameof(StockExcludeController),nameof(RemoveStockToExclude)),
                $"Stock with id {@stockId} remove from exclud list", stockId);
            return Ok();
        }
    }
}
