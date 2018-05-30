using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Texto.Api.Requests;
using Twilio;
using Twilio.AspNet.Common;
using Twilio.AspNet.Core;
using Twilio.Exceptions;
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

        [HttpPost]
        [Route("api/v1/[controller]/send")]
        public async Task<IActionResult> Send([FromBody]SendMessageRequest request)
        {
            var sid = _configuration["TwilioSmsCredentials:Sid"];
            var token = _configuration["TwilioSmsCredentials:Token"];
            var fromNumber = _configuration["TwilioSettings:FromNumber"];

            TwilioClient.Init(sid, token);

            try
            {
                var messageResource = await MessageResource.CreateAsync(from: new Twilio.Types.PhoneNumber(fromNumber),
                to: new Twilio.Types.PhoneNumber(request.ToNumber),
                body: request.Message);

                if (IsMessageSent(messageResource))
                {
                    return Ok(messageResource.Sid);
                }
                else
                {
                    return BadRequest(messageResource.Sid);
                }
            }
            catch(ApiException ex)
            {
                //TODO: Log error.
                return StatusCode(500);
            }
        }

        [HttpPost]
        [Route("api/v1/[controller]/receive")]
        public TwiMLResult Receive(SmsRequest request)
        {
            // TODO: save incoming request.
            return new TwiMLResult();
        }

        private bool IsMessageSent(MessageResource messageResource) => messageResource.Status != StatusEnum.Failed;
    }
}