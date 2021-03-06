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
    [Route("/api/[controller]")]
    public class MarketController : Controller
    {
        private readonly ILogger<MarketController> _logger;
        private readonly IAsyncQuoteSource _quoteSource;

        public MarketController(ILogger<MarketController> logger, IAsyncQuoteSource quoteSource)
        {
            _logger = logger;
            _quoteSource = quoteSource;
        }
        // GET
        [HttpGet(Name = "Get all Markets")]
        [ProducesResponseType(typeof(IList<MarketDTO>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetMarkets()
        {
            _logger.LogInformation( LogEvent.CallEndpoint(nameof(MarketController),nameof(GetMarkets)),
                "Request market list");
            var _timeFrames = await _quoteSource.Markets;

            _logger.LogInformation( LogEvent.CallEndpoint(nameof(MarketController),nameof(GetMarkets)),
                $"Response list of markets with {_timeFrames.Count} rows");
            return Ok(_timeFrames);
        }
    }
}
