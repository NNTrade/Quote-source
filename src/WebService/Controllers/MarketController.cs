using System.Threading.Tasks;
using AppCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

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
        public async Task<IActionResult> Index()
        {
            var _timeFrames = await _quoteSource.Markets;
            return Ok(_timeFrames);
        }
    }
}