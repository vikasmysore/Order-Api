using Azure.Messaging.ServiceBus;
using Infrastructure.Interfaces;
using Infrastructure.Settings;

namespace Infrastructure.Clients
{
    public class AzureServiceBusClient : IAzureServiceBusClient
    {
        private readonly string _serviceBusConnectionString;
        private readonly string _queueName;

        public AzureServiceBusClient(ServiceBusSettings settings)
        {
            _serviceBusConnectionString = settings.ConnectionString;
            _queueName = settings.QueueName;
        }

        public async Task SendOrderToQueueAsync(OrderMessage order)
        {
            var client = new ServiceBusClient(_serviceBusConnectionString);
            var sender = client.CreateSender(_queueName);

            var messageBody = System.Text.Json.JsonSerializer.Serialize(order);
            var message = new ServiceBusMessage(messageBody);

            await sender.SendMessageAsync(message);
            await sender.CloseAsync();
        }
    }
}
