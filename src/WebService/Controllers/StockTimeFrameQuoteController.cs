using System;
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
    [Route("api/stock/{stock_id}/timeframe/{timeframe}/quote")]
    public class StockTimeFrameQuoteController : Controller
    {
        private readonly ILogger<StockTimeFrameQuoteController> _logger;
        private readonly IAsyncQuoteSource _quoteSource;

        public StockTimeFrameQuoteController(ILogger<StockTimeFrameQuoteController> logger,
            IAsyncQuoteSource quoteSource)
        {
            _logger = logger;
            _quoteSource = quoteSource;
        }

        // GET /api/stock/{stock_id}/timeframe/{timeframe}/quote
        [HttpGet(Name = "Get quotes by query parameters")]
        [ProducesResponseType(typeof(CandleDTO), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetQuotesByQuery(int stock_id, int timeframe,
            [FromQuery(Name = "From Date")] DateTime from, [FromQuery(Name = "Till Date")] DateTime till)
        {
            _logger.LogInformation(
                LogEvent.CallEndpoint(nameof(StockTimeFrameQuoteController), nameof(GetQuotesByQuery)),
                "Request Quotes for: StockId {@stock}, timeframe {@timeframe}, from {@from} till {@till}", stock_id,
                timeframe, from,
                till);
            var from_d = new DateOnly(from.Year, from.Month, from.Day);
            var till_d = new DateOnly(till.Year, till.Month, till.Day);
            var quotesDTOs = await _quoteSource.Download(stock_id, timeframe, from_d, till_d);

            _logger.LogInformation(
                LogEvent.CallEndpoint(nameof(StockTimeFrameQuoteController), nameof(GetQuotesByQuery)),
                $"Response list of quotes with {quotesDTOs.Count} rows");
            return Ok(quotesDTOs);
        }
    }
}
