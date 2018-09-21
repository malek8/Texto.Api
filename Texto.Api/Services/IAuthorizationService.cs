using System.Threading.Tasks;

namespace Texto.Api.Services
{
    public interface IAuthorizationService
    {
        Task<dynamic> RequestToken(string clientId, string clientSecret, string identifier);
    }
}
