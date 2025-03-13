using Infrastructure.Interfaces;
using Infrastructure.Models;
using System.Collections.Concurrent;
using System.Text.Json;

namespace Tests.IntegrationTests.Fakes
{
    public class FakeAzureServiceBusClient : IAzureServiceBusClient
    {
        public ConcurrentQueue<string> SentMessages { get; } = new();

        public Task SendOrderToQueueAsync(OrderMessage order)
        {
            var messageBody = JsonSerializer.Serialize(order);
            SentMessages.Enqueue(messageBody);
            return Task.CompletedTask;
        }
    }
}
