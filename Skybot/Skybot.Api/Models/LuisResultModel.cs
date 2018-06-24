using System.Collections.Generic;
using Newtonsoft.Json;

namespace Skybot.Api.Models
{
    public class LuisResultModel
    {
        public IList<LuisIntent> Intents { get; set; }
        public IList<LuisEntity> Entities { get; set; }
    }

    public class LuisIntent
    {
        [JsonProperty(PropertyName = "intent")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "score")]
        public double Score { get; set; }
    }

    public class LuisEntity
    {
        [JsonProperty(PropertyName = "entity")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }
        [JsonProperty(PropertyName = "score")]
        public double Score { get; set; }
    }
}
