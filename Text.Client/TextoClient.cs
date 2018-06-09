using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Texto.Models;

namespace Texto.Client
{
    public class TextoClient : ITextoClient
    {
        private readonly string baseUri;
        private readonly HttpClient httpClient;

        public TextoClient(string baseUri)
        {
            httpClient = new HttpClient();
            this.baseUri = baseUri;
        }

        public Task GetToken(string clientId, string clientSecret, string identifier)
        {
            SetupHeaders();

            var tokenRequest = new
            {
                clientId,
                clientSecret,
                identifier
            };

            return httpClient.PostAsJsonAsync($"{baseUri}/api/token", tokenRequest);
        }

        public Task Send(SendMessageRequest message)
        {
            SetupHeaders();

            return httpClient.PostAsJsonAsync($"{baseUri}/api/text/send", message);
        }

        private void SetupHeaders()
        {
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
    }
}
