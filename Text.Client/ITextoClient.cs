using System.Threading.Tasks;
using Texto.Models;

namespace Texto.Client
{
    public interface ITextoClient
    {
        Task GetToken(string clientId, string clientSecret, string identifier);
        Task Send(SendMessageRequest message);
    }
}
