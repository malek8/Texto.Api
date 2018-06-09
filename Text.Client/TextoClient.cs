using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

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

        public Task Send(string fromNumber, string toNumber, string message)
        {
            SetupHeaders();

            var messageRequest = new
            {
                FromNumber = fromNumber,
                ToNumber = toNumber,
                Message = message
            };
            return httpClient.PostAsJsonAsync($"{baseUri}/api/text/send", messageRequest);
        }

        private void SetupHeaders()
        {
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
    }
}
