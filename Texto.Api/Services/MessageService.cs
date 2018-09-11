using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Twilio;
using Twilio.Exceptions;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Rest.Api.V2010.Account.Call;
using Twilio.Types;

namespace Texto.Api.Services
{
    public class MessageService : IMessageService
    {
        private readonly string _sid;
        private readonly string _token;

        public MessageService(IConfiguration configuration)
        {
            _sid = configuration["TwilioSmsCredentials:Sid"];
            _token = configuration["TwilioSmsCredentials:Token"];
        }

        public async Task<string> Send(string fromNumber, string toNumber, string text)
        {
            TwilioClient.Init(_sid, _token);

            try
            {
                var messageResource = await MessageResource.CreateAsync(from: new PhoneNumber(fromNumber),
                    to: new PhoneNumber(toNumber),
                    body: text);

                if (messageResource.Status != FeedbackSummaryResource.StatusEnum.Failed)
                {
                    return messageResource.Sid;
                }
            }
            catch (ApiException ex)
            {

            }
            return string.Empty;
        }
    }
}
