using System;
using System.Threading.Tasks;
using AppCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace WebService.Controllers
{
  [ApiController]
  [Route("api/stock/{stock_id}/timeframe/{timeframe}/[controller]")]
  public class QuoteController : Controller
  {
      private readonly ILogger<MarketController> _logger;
      private readonly IAsyncQuoteSource _quoteSource;

      public QuoteController(ILogger<MarketController> logger, IAsyncQuoteSource quoteSource)
      {
          _logger = logger;
          _quoteSource = quoteSource;
      }

      // GET /api/stock/{stock_id}/{timeframe}
      [HttpGet(Name = "Get quotes by query parametrs")]
      public async Task<IActionResult> Index(int stock_id,int timeframe,[FromQuery(Name = "From Date")]DateTime from,[FromQuery(Name = "Till Date")]DateTime till)
      {
          var from_d = new DateOnly(from.Year, from.Month, from.Day);
          var till_d = new DateOnly(till.Year, till.Month, till.Day);
          var quotesDTOs = await _quoteSource.Download(stock_id, timeframe, from_d, till_d);
          return Ok(quotesDTOs);
      }
  }
}
