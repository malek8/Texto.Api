using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Texto.Api.Models;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace Texto.Api.Controllers
{
    //[Route("api/[controller]")]
    public class MessengerController : Controller
    {
        private readonly IConfiguration _configuration;

        public MessengerController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [Route("api/v1/[controller]/send")]
        [HttpPost]
        public void Send([FromBody]MessageModel model)
        {
            var sid = _configuration["TwilioSmsCredentials:Sid"];
            var token = _configuration["TwilioSmsCredentials:Token"];
            var fromNumber = _configuration["TwilioSettings:FromNumber"];

            TwilioClient.Init(sid, token);

            var result = MessageResource.Create(from: new Twilio.Types.PhoneNumber(fromNumber),
                to: new Twilio.Types.PhoneNumber(model.Number),
                body: model.Message);
        }
    }
}