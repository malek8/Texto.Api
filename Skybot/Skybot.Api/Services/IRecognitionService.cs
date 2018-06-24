using System.Threading.Tasks;
using Skybot.Models.Skybot;

namespace Skybot.Api.Services
{
    public interface IRecognitionService
    {
        Task<RecognitionResult> Process(string message);
    }
}
