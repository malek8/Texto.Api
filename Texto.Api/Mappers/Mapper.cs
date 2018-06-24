using Skybot.Models.Texto;
using Twilio.AspNet.Common;

namespace Texto.Api.Mappers
{
    public static class Mapper
    {
        public static TextMessage Map(SmsRequest smsRequest)
        {
            return new TextMessage
            {
                SmsSid = smsRequest.SmsSid,
                Body = smsRequest.Body,
                MessageStatus = smsRequest.MessageStatus,
                AccountSid = smsRequest.AccountSid,
                From = smsRequest.From,
                To = smsRequest.To,
                FromCity = smsRequest.FromCity,
                FromState = smsRequest.FromState,
                FromZip = smsRequest.FromZip,
                FromCountry = smsRequest.FromCountry,
                ToCity = smsRequest.ToCity,
                ToState = smsRequest.ToState,
                ToZip = smsRequest.ToZip,
                ToCountry = smsRequest.ToCountry
            };
        }
    }
}
