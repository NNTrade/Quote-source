using System.Threading.Tasks;
using AppCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace WebService.Controllers
{
    [ApiController]
    [Route("/api/market/{market_id}/[controller]")]
    public class StockController : ControllerBase
    {
        private readonly ILogger<MarketController> _logger;
        private readonly IAsyncQuoteSource _quoteSource;

        public StockController(ILogger<MarketController> logger, IAsyncQuoteSource quoteSource)
        {
            _logger = logger;
            _quoteSource = quoteSource;
        }

        // GET /api/market/{market_id}/stock?code=
        [HttpGet(Name = "Get Stocks by query parametrs")]
        public async Task<IActionResult> GetStockByQuery(int market_id, [FromQuery]string? code = null)
        {
            _logger.LogInformation( LogEvent.CallEndpoint(nameof(StockController),nameof(GetStockByQuery)),
                "Request stock for: MarketId {@market_id}, Code {@code}", market_id,code);
            var _stockDtos = await _quoteSource.StockSearch(market_id, code ?? "");
            return Ok(_stockDtos);
        }
    }
}
