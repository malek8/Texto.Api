using System.Threading.Tasks;
using Skybot.Api.Models;

namespace Skybot.Api.Services
{
    public interface IRecognitionService
    {
        Task<RecognitionResult> Process(string message);
    }
}
