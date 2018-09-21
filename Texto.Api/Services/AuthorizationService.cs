using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Texto.Api.Services
{
    public class AuthorizationService : IAuthorizationService
    {
        private static HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public AuthorizationService(IConfiguration configuration)
        {
            _configuration = configuration;
            _httpClient = new HttpClient();

            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<dynamic> RequestToken(string clientId, string clientSecret, string identifier)
        {
            var requestContent = new
            {
                grant_type = "client_credentials",
                client_id = clientId,
                client_secret = clientSecret,
                audience = identifier
            };

            var response = await _httpClient.PostAsJsonAsync($"https://{_configuration["Auth0:Domain"]}/oauth/token", requestContent);

            return JsonConvert.DeserializeObject<dynamic>(await response.Content.ReadAsStringAsync());
        }
    }
}
