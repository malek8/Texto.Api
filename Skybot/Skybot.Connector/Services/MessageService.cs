using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;

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
            return queueClient.CompleteAsync(message.SystemProperties.LockToken);
        }
    }
}
