using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Texto.Api.Services
{
    public class BusService : IBusService
    {
        private readonly IConfiguration _configuration;

        public BusService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Task PublishAsync<T>(T item)
        {
            var busConfigs = _configuration.GetSection("AzureBus");
            var queueClient = new QueueClient(busConfigs["ConnectionString"], busConfigs["QueueName"]);

            return queueClient.SendAsync(new Message
            {
                To = "textBrokers",
                Body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(item))
            });
        }
    }
}
