using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TwitterStreamer.Services.Interfaces;

namespace TwitterStreamer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class Top10HashTagController : ControllerBase
    {
        private readonly ILogger<Top10HashTagController> _logger;
        private readonly IHashTagCounterService _hastTagCounterService;

        public Top10HashTagController(ILogger<Top10HashTagController> logger, IHashTagCounterService hastTagCounterService)
        {
            _logger = logger;
            _hastTagCounterService = hastTagCounterService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var results = _hastTagCounterService.GetTop10HashTag();
            return Ok(results);
        }
    }
}
