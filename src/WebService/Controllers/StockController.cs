using System.Runtime.InteropServices;
using System.Threading.Tasks;
using AppCore;
using database.entity;
using interactor;
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
        public async Task<IActionResult> Index(int market_id, [FromQuery]string? code = null)
        {
            var _stockDtos = await _quoteSource.StockSearch(market_id, code ?? "");
            return Ok(_stockDtos);
        }
    }
}