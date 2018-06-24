using System.Threading.Tasks;

namespace Texto.Api.Services
{
    public interface IBusService
    {
        Task PublishAsync<T>(T item);
    }
}
