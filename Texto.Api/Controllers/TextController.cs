using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Texto.Api.Requests;
using Texto.Api.Services;
using Texto.Data;
using Texto.Models;
using Twilio;
using Twilio.AspNet.Common;
using Twilio.AspNet.Core;
using Twilio.Exceptions;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Rest.Api.V2010.Account.Call;

namespace Texto.Api.Controllers
{
    [Produces("application/json")]
    //[Route("api/[controller]")]
    public class TextController : Controller
    {
        private readonly IConfiguration configuration;
        private readonly IContactsContext contactsContext;
        private IContactsService ContactsService { get; }

        public TextController(IConfiguration configuration, IContactsContext contactsContext, IContactsService contactsService)
        {
            this.configuration = configuration;
            this.contactsContext = contactsContext;
            this.ContactsService = contactsService;
        }

        [Route("api/[controller]/send")]
        [HttpPost("{request}")]
        public async Task<IActionResult> Send([FromBody]SendMessageRequest request)
        {
            var sid = configuration["TwilioSmsCredentials:Sid"];
            var token = configuration["TwilioSmsCredentials:Token"];
            var fromNumber = configuration["TwilioSettings:FromNumber"];

            SaveMessage(request);

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

        [Route("api/[controller]/receive")]
        [HttpPost("{request}")]
        public TwiMLResult Receive(SmsRequest request)
        {
            // TODO: save incoming request.
            return new TwiMLResult();
        }

        private void SaveMessage(SendMessageRequest messageRequest)
        {
            var contact = ContactsService.GetByPhoneNumber(messageRequest.ToNumber);
            if (contact == null)
            {
                contact = new Contact
                {
                    Info = new ContactInfo
                    {
                        Number = messageRequest.ToNumber
                    }
                };

                ContactsService.Add(contact);
            }

            ContactsService.SendMessage(contact.Id, new Message
            {
                From = messageRequest.FromNumber,
                Text = messageRequest.Message,
                CreatedOn = DateTime.UtcNow
            });
        }

        private static bool IsMessageSent(MessageResource messageResource) => messageResource.Status != FeedbackSummaryResource.StatusEnum.Failed;
    }
}