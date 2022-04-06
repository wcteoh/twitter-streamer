using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TwitterStreamer.Services.Interfaces;

namespace TwitterStreamer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MessageTotalController : ControllerBase
    {
        private readonly ILogger<MessageTotalController> _logger;
        private readonly IMessageTotalService _message;

        public MessageTotalController(ILogger<MessageTotalController> logger, IMessageTotalService message)
        {
            _logger = logger;
            _message = message; 
        }

        [HttpGet]
        public IActionResult Get()
        {
            var result = _message.GetMessageTotal();
            return Ok($"API Called success. {result}");
        }

    }
}
