using Skybot.Models.Skybot;

namespace Skybot.Api.Services.IntentsServices
{
    public interface IIntentService
    {
        string Process(LuisResultModel model);
    }
}
