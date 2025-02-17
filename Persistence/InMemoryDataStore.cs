using Persistence.Entities;
using Persistence.RequestModels;
using Persistence.ResponseModels;

namespace Persistence
{
    public class InMemoryDataStore
    {
        private readonly IList<OrderEntity> orders = new List<OrderEntity>();

        public async Task<CreateOrderResponse> CreateOrder(CreateOrderRequest orderRequest)
        {
            OrderEntity order = null;
            CreateOrderResponse orderResponse = new CreateOrderResponse();

            if (orderRequest != null)
            {
                order = new OrderEntity()
                {
                    PartitionKey = orderRequest.Email,
                    OrderNo = Guid.NewGuid(),
                    Email = orderRequest.Email,
                    ItemCount = orderRequest.ItemCount,
                    ItemId = orderRequest.ItemId
                };

                orders.Add(order);

                orderResponse.OrderNo = order.OrderNo;
                orderResponse.Email = order.Email;
                orderResponse.ItemCount = order.ItemCount;
                orderResponse.ItemId = order.ItemId;
            }

            return await Task.FromResult(orderResponse ?? new CreateOrderResponse());
        }

        public async Task<IList<CreateOrderResponse>> GetAllOrders()
        {
            return await Task.FromResult(orders.Select(o => new CreateOrderResponse
            {
                OrderNo = o.OrderNo,
                Email = o.Email,
                ItemCount = o.ItemCount,
                ItemId = o.ItemId
            }).ToList());
        }

        public async Task<CreateOrderResponse> GetOrderByOrderNo(Guid orderNo)
        {
            var order = orders.FirstOrDefault(o => o.OrderNo == orderNo);
            CreateOrderResponse orderResponse = new CreateOrderResponse();
            if (order == null)
            {
                return orderResponse;
            }

            orderResponse.OrderNo = order.OrderNo;
            orderResponse.Email = order.Email;
            orderResponse.ItemCount = order.ItemCount;
            orderResponse.ItemId = order.ItemId;

            return await Task.FromResult(orderResponse);
        }
    }
}
