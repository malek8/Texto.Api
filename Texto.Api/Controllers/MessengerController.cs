using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Texto.Api.Models;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using static Twilio.Rest.Api.V2010.Account.Call.FeedbackSummaryResource;

namespace Texto.Api.Controllers
{
    [Produces("application/json")]
    public class MessengerController : Controller
    {
        private readonly IConfiguration _configuration;

        public MessengerController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [Route("api/v1/[controller]/send")]
        [HttpPost]
        public async Task<IActionResult> Send([FromBody]SendMessageModel model)
        {
            var sid = _configuration["TwilioSmsCredentials:Sid"];
            var token = _configuration["TwilioSmsCredentials:Token"];
            var fromNumber = _configuration["TwilioSettings:FromNumber"];

            TwilioClient.Init(sid, token);

            var messageResource = await MessageResource.CreateAsync(from: new Twilio.Types.PhoneNumber(fromNumber),
                to: new Twilio.Types.PhoneNumber(model.ToNumber),
                body: model.Message);

            if (IsMessageSent(messageResource))
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

        private bool IsMessageSent(MessageResource messageResource) => messageResource.Status != StatusEnum.Failed;
    }
}