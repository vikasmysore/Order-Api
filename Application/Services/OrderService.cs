using Application.Interfaces;
using AutoMapper;
using Domain.Models;
using Infrastructure.Interfaces;
using Infrastructure.Models;
using Persistence;
using Persistence.Repository.Interfaces;
using Persistence.RequestModels;

namespace Application.Services
{
    public class OrderService(InMemoryDataStore dataStore, IMapper mapper, IAzureServiceBusClient serviceBusClient,
        IOrderRepository orderRepository) : IOrderService
    {
        public async Task<GetOrder> CreateOrder(Order order)
        {
            var orderRequest = mapper.Map<CreateOrderRequest>(order);

            //var response = await dataStore.CreateOrder(orderRequest);

            var cosmosResponse = await orderRepository.CreateOrder(orderRequest);

            var getOrder = mapper.Map<GetOrder>(cosmosResponse);

            SendMessage(getOrder);

            return getOrder;
        }

        public async Task<IList<GetOrder>> GetAllOrders()
        {
            //var response = await dataStore.GetAllOrders();

            var cosmosResponse = await orderRepository.GetAllOrders();

            return cosmosResponse.Select(r => mapper.Map<GetOrder>(r)).ToList();
        }

        public async Task<GetOrder> GetOrderByOrderNo(string orderNo)
        {
            //var response = await dataStore.GetOrderByOrderNo(Guid.Parse(orderNo));

            var cosmosResponse = await orderRepository.GetOrderByOrderNo(orderNo);

            return mapper.Map<GetOrder>(cosmosResponse);
        }

        private async void SendMessage(GetOrder order)
        {
            var orderMessage = mapper.Map<OrderMessage>(order);

            await serviceBusClient.SendOrderToQueueAsync(orderMessage);
        }
    }
}
