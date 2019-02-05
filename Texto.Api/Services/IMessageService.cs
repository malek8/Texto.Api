using System.Threading.Tasks;

namespace Texto.Api.Services
{
    public interface IMessageService
    {
        Task<string> Send(string toNumber, string text);
    }
}
