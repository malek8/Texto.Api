using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Skybot.Models.Skybot;

namespace Skybot.Api.Services
{
    public class RecognitionService : IRecognitionService
    {
        private readonly IConfiguration configuration;

        public RecognitionService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async Task<RecognitionResult> Process(string message)
        {
            var recognitionIntents = await GetQueryIntents(message);

            return await ProcessIntents(recognitionIntents);
        }

        private Task<RecognitionResult> ProcessIntents(LuisResultModel model)
        {
            if (model != null)
            {
                var intent = model.Intents?.OrderByDescending(x => x.Score).FirstOrDefault();
                if (intent?.Score > 0.8)
                {
                    if (intent.Name.Equals("Translate"))
                    {
                        return (Task<RecognitionResult>) Task.CompletedTask;
                    }
                }
            }

            return null;
        }

        private async Task<string> CallSkybotApp(string query)
        {
            var httpClient = new HttpClient();

            var encodedQuery = HttpUtility.UrlEncode(query);

            try
            {
                var response = await httpClient.GetAsync($"{configuration["LuisApp:Endpoint"]}?subscription-key={configuration["LuisApp:Key"]}&verbose=true&timezoneOffset=0&q={encodedQuery}");
                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception)
            {
                // ignored
            }

            return string.Empty;
        }

        private async Task<LuisResultModel> GetQueryIntents(string query)
        {
            var serializedResult = await CallSkybotApp(query);

            return !string.IsNullOrEmpty(serializedResult) ? JsonConvert.DeserializeObject<LuisResultModel>(serializedResult) : null;
        }

        private static IDictionary<string, string> IntentsDictionary => new Dictionary<string, string>
        {
            {"None", "None"},
            {"Who is", "WhoIs"},
            {"Translate", "Translate"}
        };
    }
}
