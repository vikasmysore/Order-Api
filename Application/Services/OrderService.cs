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
            if (cosmosResponse.IsError)
            {
                throw new Exception(cosmosResponse.ErrorResponse);
            }

            var getOrder = mapper.Map<GetOrder>(cosmosResponse.Content);

            SendMessage(getOrder);

            return getOrder;
        }

        public async Task<IList<GetOrder>> GetAllOrders(string email)
        {
            //var response = await dataStore.GetAllOrders();

            var cosmosResponse = await orderRepository.GetAllOrders(email);
            if (cosmosResponse.IsError)
            {
                throw new Exception(cosmosResponse.ErrorResponse);
            }

            return cosmosResponse.Content.Orders.Select(r => mapper.Map<GetOrder>(r)).ToList();
        }

        public async Task<GetOrder> GetOrderByOrderNo(string orderNo, string email)
        {
            //var response = await dataStore.GetOrderByOrderNo(Guid.Parse(orderNo));

            var cosmosResponse = await orderRepository.GetOrderByOrderNo(orderNo, email);
            if (cosmosResponse.IsError)
            {
                throw new Exception(cosmosResponse.ErrorResponse);
            }
            return mapper.Map<GetOrder>(cosmosResponse.Content);
        }

        private async void SendMessage(GetOrder order)
        {
            var orderMessage = mapper.Map<OrderMessage>(order);

            await serviceBusClient.SendOrderToQueueAsync(orderMessage);
        }
    }
}
