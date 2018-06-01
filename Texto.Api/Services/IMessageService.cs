using System.Threading.Tasks;

namespace Texto.Api.Services
{
    public interface IMessageService
    {
        Task<string> Send(string fromNumber, string toNumber, string text);
    }
}
