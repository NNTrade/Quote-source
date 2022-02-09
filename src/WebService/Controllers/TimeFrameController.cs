using System.Threading.Tasks;
using AppCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace WebService.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class TimeFrameController : Controller
    {
        private readonly ILogger<TimeFrameController> _logger;
        private readonly IAsyncQuoteSource _quoteSource;

        public TimeFrameController(ILogger<TimeFrameController> logger, IAsyncQuoteSource quoteSource)
        {
            _logger = logger;
            _quoteSource = quoteSource;
        }
        // GET
        [HttpGet(Name = "Get all TimeFrames")]
        public async Task<IActionResult> GetAllTimeframe()
        {
            _logger.LogInformation( LogEvent.CallEndpoint(nameof(TimeFrameController),nameof(GetAllTimeframe)),
                "Request timeframe list");
            var _timeFrames = await _quoteSource.TimeFrames;
            return Ok(_timeFrames);
        }
    }
}
