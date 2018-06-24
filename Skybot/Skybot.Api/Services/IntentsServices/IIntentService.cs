using Skybot.Api.Models;

namespace Skybot.Api.Services.IntentsServices
{
    public interface IIntentService
    {
        string Process(LuisResultModel model);
    }
}
