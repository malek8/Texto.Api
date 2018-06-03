using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Texto.Api.Requests;
using Texto.Api.Services;
using Twilio.AspNet.Common;
using Twilio.AspNet.Core;

namespace Texto.Api.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class TextController : Controller
    {
        private readonly IMessageService messageService;

        public TextController(IConfiguration configuration, IMessageService messageService)
        {
            this.messageService = messageService;
        }

        [Route("send")]
        [HttpPost("{request}")]
        [Authorize]
        public async Task<IActionResult> Send([FromBody]SendMessageRequest request)
        {
            var messageSid = await messageService.Send(request.FromNumber, request.ToNumber, request.Message);

            if (string.IsNullOrEmpty(messageSid))
            {
                return BadRequest();
            }
            else
            {
                return Ok(messageSid);
            }
        }

        [Route("receive")]
        [HttpPost("{request}")]
        public TwiMLResult Receive(SmsRequest request)
        {
            // TODO: save incoming request.
            return new TwiMLResult();
        }
    }
}