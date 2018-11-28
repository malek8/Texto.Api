using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Texto.Api.Services;
using Twilio.AspNet.Common;
using Twilio.AspNet.Core;
using Texto.Api.Models;
using IAuthorizationService = Texto.Api.Services.IAuthorizationService;

namespace Texto.Api.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class TextController : Controller
    {
        private readonly IMessageService _messageService;
        private readonly IBusService _busService;
        private readonly IAuthorizationService _authorizationService;
        private readonly IConfiguration _configuration;
        private readonly ILogger<TextController> _logger;

        public TextController(IConfiguration configuration, IMessageService messageService,
            IBusService busService, IAuthorizationService authorizationService,
            ILogger<TextController> logger)
        {
            _messageService = messageService;
            _configuration = configuration;
            _busService = busService;
            _authorizationService = authorizationService;
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
        public async Task<TwiMLResult> Receive(SmsRequest request)
        {
            var receivingKey = _configuration["Settings:ReceivingKey"];
            if (Request.Query.ContainsKey("key"))
            {
                if (Request.Query["key"][0].Equals(receivingKey))
                {
                    var authorizedNumbers = _configuration.GetSection("AuthorizedNumbers:Numbers").Get<string[]>();

                    if (authorizedNumbers.Contains(request.From))
                    {
                        var message = new
                        {
                            request.From,
                            request.To,
                            request.Body
                        };
                        await _busService.PublishAsync(message);
                        return new TwiMLResult();
                    }

                    _logger.LogInformation($"Received message from unauthorized number {request.From} - Country: {request.FromCountry} - Message: {request.Body}");
                }
            }

            _logger.LogWarning("Unauthorized receive request");
            return new TwiMLResult("Error");
        }

        [AllowAnonymous]
        [Route("token")]
        [HttpPost]
        public async Task<IActionResult> RequestToken(string clientId, string clientSecret, string identifier)
        {
            var token = await _authorizationService.RequestToken(clientId, clientSecret, identifier);

            return token == null ? StatusCode(StatusCodes.Status401Unauthorized) : (IActionResult) Ok(token);
        }
    }
}