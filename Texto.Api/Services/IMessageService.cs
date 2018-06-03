using System.Threading.Tasks;
using Twilio.AspNet.Common;

namespace Texto.Api.Services
{
    public interface IMessageService
    {
        Task<string> Send(string fromNumber, string toNumber, string text);

        Task Receive(SmsRequest request);
    }
}
