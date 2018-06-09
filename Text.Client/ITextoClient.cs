using System.Threading.Tasks;

namespace Texto.Client
{
    public interface ITextoClient
    {
        Task GetToken(string clientId, string clientSecret, string identifier);
        Task Send(string fromNumber, string toNumber, string message);
    }
}
