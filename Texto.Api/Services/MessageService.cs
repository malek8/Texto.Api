using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Rest.Api.V2010.Account.Call;
using Twilio.Types;

namespace Texto.Api.Services
{
    public class MessageService : IMessageService
    {
        private readonly string _sid;
        private readonly string _token;
        private readonly ILogger<MessageService> _logger;

        public MessageService(IConfiguration configuration, ILogger<MessageService> logger)
        {
            _sid = configuration["TwilioSmsCredentials:Sid"];
            _token = configuration["TwilioSmsCredentials:Token"];
            _logger = logger;
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
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in MessageService, failed to send message to: {toNumber} - From: {fromNumber} - Message: {text}");
            }
            return string.Empty;
        }
    }
}
