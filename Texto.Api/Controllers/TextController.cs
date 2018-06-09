using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using Texto.Api.Services;
using Texto.Models;
using Twilio.AspNet.Common;
using Twilio.AspNet.Core;

namespace Texto.Api.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class TextController : Controller
    {
        private readonly IMessageService messageService;
        private readonly IConfiguration configuration;

        public TextController(IConfiguration configuration, IMessageService messageService)
        {
            this.messageService = messageService;
            this.configuration = configuration;
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

            return Ok(messageSid);
        }

        [Route("receive")]
        [HttpPost("{request}")]
        public async Task<TwiMLResult> Receive(SmsRequest request)
        {
            var receivingKey = configuration["Settings:ReceivingKey"];
            if (Request.Query.ContainsKey("key"))
            {
                if (Request.Query["key"][0].Equals(receivingKey))
                {
                    await messageService.Receive(request);

                    var queueClient = new QueueClient(configuration["AzureBus:ConnectionString"], configuration["AzureBus:QueueName"]);
                    await queueClient.SendAsync(new Microsoft.Azure.ServiceBus.Message
                    {
                        To = "textBrokers",
                        Body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(request))
                    });
                }
            }

            return new TwiMLResult();
        }

        [AllowAnonymous]
        [Route("token")]
        [HttpPost]
        public async Task<IActionResult> RequestToken(string clientId, string clientSecret, string identifier)
        {
            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var requestContent = new
            {
                grant_type = "client_credentials",
                client_id = clientId,
                client_secret = clientSecret,
                audience = identifier
            };

            var response = await httpClient.PostAsJsonAsync($"https://{configuration["Auth0:Domain"]}/oauth/token", requestContent);
            var responseContent = await response.Content.ReadAsStringAsync();

            var tokenObject = JsonConvert.DeserializeObject<dynamic>(responseContent);

            return tokenObject == null ? StatusCode((int)HttpStatusCode.Unauthorized) : (IActionResult) Ok(tokenObject.access_token);
        }
    }
}