using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Texto.Api.Services;
using Texto.Api.Models;

namespace Texto.Api.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class TextController : Controller
    {
        private readonly IMessageService _messageService;
        private readonly IBusService _busService;
        private readonly ILogger<TextController> _logger;

        public TextController(IMessageService messageService, IBusService busService, ILogger<TextController> logger)
        {
            _messageService = messageService;
            _busService = busService;
            _logger = logger;
        }

        [Route("send")]
        [HttpPost("{request}")]
        [Authorize]
        public async Task<IActionResult> Send([FromBody]SendMessageRequest request)
        {
            var messageSid = await _messageService.Send(request.FromNumber, request.ToNumber, request.Message);

            if (string.IsNullOrEmpty(messageSid))
            {
                _logger.LogWarning("Message was not sent");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return Ok(messageSid);
        }

        [Route("receive")]
        [HttpPost("{request}")]
        [Authorize]
        public async Task<IActionResult> Receive([FromBody]SendMessageRequest request)
        {
            await _busService.PublishAsync(request);
            return Ok();
        }
    }
}