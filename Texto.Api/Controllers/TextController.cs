using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Texto.Api.Requests;
using Texto.Api.Services;
using Twilio.AspNet.Common;
using Twilio.AspNet.Core;

namespace Texto.Api.Controllers
{
    [Produces("application/json")]
    //[Route("api/[controller]")]
    public class TextController : Controller
    {
        private readonly IConfiguration configuration;
        private readonly IMessageService messageService;

        public TextController(IConfiguration configuration, IMessageService messageService)
        {
            this.configuration = configuration;
            this.messageService = messageService;
        }

        [Route("api/[controller]/send")]
        [HttpPost("{request}")]
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

        [Route("api/[controller]/receive")]
        [HttpPost("{request}")]
        public TwiMLResult Receive(SmsRequest request)
        {
            // TODO: save incoming request.
            return new TwiMLResult();
        }
    }
}