using System.Diagnostics.CodeAnalysis;
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
    public class TimeFrameController : Controller
    {
        private readonly ILogger _logger;
        private readonly IAsyncQuoteSource _quoteSource;

        public TimeFrameController(ILogger<TimeFrameController> logger, IAsyncQuoteSource quoteSource)
        {
            _logger = logger;
            _quoteSource = quoteSource;
        }
        // GET
        [HttpGet(Name = "Get all TimeFrames")]
        [ProducesResponseType(typeof(TimeFrameDTO), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAllTimeframe()
        {
            _logger.LogInformation( LogEvent.CallEndpoint(nameof(TimeFrameController),nameof(GetAllTimeframe)),
                "Request timeframe list");
            var _timeFrames = await _quoteSource.TimeFrames;

            _logger.LogInformation( LogEvent.CallEndpoint(nameof(TimeFrameController),nameof(GetAllTimeframe)),
                $"Response list of timeframes with {_timeFrames.Count} rows");
            return Ok(_timeFrames);
        }
    }
}
