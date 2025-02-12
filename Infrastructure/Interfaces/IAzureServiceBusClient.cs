using Infrastructure.Models;

namespace Infrastructure.Interfaces
{
    public interface IAzureServiceBusClient
    {
        public Task SendOrderToQueueAsync(OrderMessage order);
    }
}
