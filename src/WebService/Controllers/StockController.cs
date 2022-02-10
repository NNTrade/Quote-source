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
    [Route("/api/market/{market_id}/[controller]")]
    public class StockController : ControllerBase
    {
        private readonly ILogger<StockController> _logger;
        private readonly IAsyncQuoteSource _quoteSource;

        public StockController(ILogger<StockController> logger, IAsyncQuoteSource quoteSource)
        {
            _logger = logger;
            _quoteSource = quoteSource;
        }

        // GET /api/market/{market_id}/stock?code=
        [HttpGet(Name = "Get Stocks by query parametrs")]
        [ProducesResponseType(typeof(StockDTO), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetStockByQuery(int market_id, [FromQuery]string? code = null)
        {
            _logger.LogInformation( LogEvent.CallEndpoint(nameof(StockController),nameof(GetStockByQuery)),
                "Request stock for: MarketId {@market_id}, Code {@code}", market_id,code);
            var _stockDtos = await _quoteSource.StockSearch(market_id, code ?? "");
            return Ok(_stockDtos);
        }
    }
}
