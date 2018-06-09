using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Texto.Models;
using Twilio;
using Twilio.AspNet.Common;
using Twilio.Exceptions;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Rest.Api.V2010.Account.Call;
using Twilio.Types;

namespace Texto.Api.Services
{
    public class MessageService : IMessageService
    {
        private readonly IContactsService contactsService;
        private readonly string sid;
        private readonly string token;

        public MessageService(IConfiguration configuration, IContactsService contactsService)
        {
            this.contactsService = contactsService;

            sid = configuration["TwilioSmsCredentials:Sid"];
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

                await AddMessage(fromNumber, toNumber, text, messageResource.Sid, MessageDirection.Incoming);

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

        public async Task Receive(SmsRequest request)
        {
            await AddMessage(request.To, request.From, request.Body, request.SmsSid, MessageDirection.Outgoing,
                new Address
                {
                    City = request.FromCity,
                    Province = request.FromState,
                    Country = request.FromCountry,
                    PostalCode = request.FromZip
                });
        }

        private async Task AddMessage(string fromNumber, string toNumber, string text, string messageSid, string direction, Address address = null)
        {
            var contact = await contactsService.GetByPhoneNumber(toNumber);
            if (contact == null)
            {
                contact = new Contact
                {
                    Info = new ContactInfo
                    {
                        Number = toNumber
                    }
                };

                await contactsService.Add(contact);
            }

            if (address != null)
            {
                await contactsService.UpdateAddress(contact.Id, address);
            }

            await contactsService.AddMessage(contact.Id, new Message
            {
                From = fromNumber,
                To = toNumber,
                Text = text,
                CreatedOn = DateTime.UtcNow,
                Direction = direction,
                Sid = messageSid
            });
        }
    }
}
