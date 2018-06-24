using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Skybot.Connector.Services
{
    public class MessageService : IMessageService
    {
        private readonly IQueueClient queueClient;

        public MessageService(IConfiguration configuration)
        {
            queueClient = new QueueClient(configuration["AzureBus:ConnectionString"], configuration["AzureBus:ReceivingQ"]);
        }

        public void ProcessIncomingMessages()
        {
            var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
            {
                MaxConcurrentCalls = 1,
                AutoComplete = false
            };

            queueClient.RegisterMessageHandler(ProcessMessagesAsync, messageHandlerOptions);
        }

        private static Task ExceptionReceivedHandler(ExceptionReceivedEventArgs arg)
        {
            throw new NotImplementedException();
        }

        private Task ProcessMessagesAsync(Message message, CancellationToken token)
        {
            var messageAsString = Encoding.UTF8.GetString(message.Body);
            var messageObject = JsonConvert.DeserializeObject<dynamic>(messageAsString);
            var messageQuery = messageObject.Body.ToString();

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            try
            {
                var requestContent = new
                {
                    query = messageQuery
                };
                httpClient.PostAsJsonAsync("https://localhost:44327/api/Skybot/process", requestContent, token);
            }
            catch (Exception)
            {
                // ignored
            }

            return queueClient.CompleteAsync(message.SystemProperties.LockToken);
        }
    }
}
