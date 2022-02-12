using System.Net;
using AppCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using LogEvent = WebService.Logging.LogEvent;

namespace WebService.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class AdminController : Controller
    {
        private readonly IIniter _initer;
        private readonly ILogger<AdminController> _logger;

        public AdminController(IIniter initer, ILogger<AdminController> logger)
        {
            _initer = initer;
            _logger = logger;
        }

        // GET
        [HttpPost(Name="Reinit database")]
        public IActionResult Reinit()
        {
            _logger.LogInformation(LogEvent.CallEndpoint(nameof(AdminController), nameof(Reinit)),
                "Request reinit database");
            _initer.Reinit();
            return Ok();
        }
    }
}
