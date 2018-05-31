using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Texto.Api.Requests;
using Twilio;
using Twilio.AspNet.Common;
using Twilio.AspNet.Core;
using Twilio.Exceptions;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Rest.Api.V2010.Account.Call;

namespace Texto.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class TextController : Controller
    {
        private readonly IConfiguration configuration;

        public TextController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        [HttpPost("{request}")]
        public async Task<IActionResult> Send([FromBody]SendMessageRequest request)
        {
            var sid = configuration["TwilioSmsCredentials:Sid"];
            var token = configuration["TwilioSmsCredentials:Token"];
            var fromNumber = configuration["TwilioSettings:FromNumber"];

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
            catch (ApiException ex)
            {
                //TODO: Log error.
                return StatusCode(500);
            }
        }

        [HttpPost("{request}")]
        public TwiMLResult Receive(SmsRequest request)
        {
            // TODO: save incoming request.
            return new TwiMLResult();
        }

        private static bool IsMessageSent(MessageResource messageResource) => messageResource.Status != FeedbackSummaryResource.StatusEnum.Failed;
    }
}