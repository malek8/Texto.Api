using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Texto.Models;
using Twilio;
using Twilio.Exceptions;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Rest.Api.V2010.Account.Call;
using Twilio.Types;

namespace Texto.Api.Services
{
    public class MessageService : IMessageService
    {
        private readonly IConfiguration configuration;
        private readonly IContactsService contactsService;
        private readonly string sid;
        private readonly string token;

        public MessageService(IConfiguration configuration, IContactsService contactsService)
        {
            this.configuration = configuration;
            this.contactsService = contactsService;

            sid = this.configuration["TwilioSmsCredentials:Sid"];
            token = configuration["TwilioSmsCredentials:Token"];
        }

        public async Task<string> Send(string fromNumber, string toNumber, string text)
        {
            TwilioClient.Init(sid, token);

            try
            {
                var messageResource = await MessageResource.CreateAsync(from: new PhoneNumber(fromNumber),
                    to: new PhoneNumber(toNumber),
                    body: text);

                SaveMessage(fromNumber, toNumber, text, messageResource);

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

        private void SaveMessage(string fromNumber, string toNumber, string text, MessageResource messageResource)
        {
            var contact = contactsService.GetByPhoneNumber(toNumber);
            if (contact == null)
            {
                contact = new Contact
                {
                    Info = new ContactInfo
                    {
                        Number = toNumber
                    }
                };

                contactsService.Add(contact);
            }

            contactsService.SendMessage(contact.Id, new Message
            {
                From = fromNumber,
                Text = text,
                CreatedOn = DateTime.UtcNow,
                Direction = MessageDirection.Incoming,
                Sid = messageResource.Sid,
                Status = messageResource.Status == FeedbackSummaryResource.StatusEnum.Failed ? "failed" : "sent"
            });
        }
    }
}
