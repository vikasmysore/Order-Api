using CrossCutting.ResultResponse;
using Persistence.Entities;
using Persistence.RequestModels;
using Persistence.ResponseModels;
using static Persistence.ResponseModels.CreateOrderResponse;
using static Persistence.ResponseModels.GetAllOrdersReponse;

namespace Persistence
{
    public class InMemoryDataStore
    {
        private readonly IList<OrderEntity> orders = new List<OrderEntity>();

        public async Task<CreateOrderResponse> CreateOrder(CreateOrderRequest orderRequest)
        {
            OrderEntity order = null;

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

                var response = new CreateOrderResponse(new CreateOrderResponseEntity()
                {
                    OrderNo = order.OrderNo,
                    Email = order.Email,
                    ItemCount = order.ItemCount,
                    ItemId = order.ItemId
                });

                return await Task.FromResult(response);
            }

            return await Task.FromResult(new CreateOrderResponse(new ErrorResponse("Could not create the order.")));
        }

        public async Task<GetAllOrdersReponse> GetAllOrders(string email)
        {
            var response = orders.Where(o => o.Email == email).ToList();

            if (response == null || response.Count == 0)
            {
                return new GetAllOrdersReponse(new ErrorResponse("No orders to show."));
            }

            var orderList = response.Select(r => new CreateOrderResponseEntity() { OrderNo = r.OrderNo, Email = r.Email, ItemCount = r.ItemCount, ItemId = r.ItemId });

            return await Task.FromResult(new GetAllOrdersReponse(new GetAllOrdersReponseEntity() { Orders = orderList }));
        }

        public async Task<CreateOrderResponse> GetOrderByOrderNo(Guid orderNo, string email)
        {
            var order = orders.FirstOrDefault(o => o.OrderNo == orderNo && o.Email == email);

            if (order == null)
            {
                return await Task.FromResult(new CreateOrderResponse(new ErrorResponse("Order Not Found")));
            }

            var orderResponse = new CreateOrderResponse(new CreateOrderResponseEntity()
            {
                OrderNo = order.OrderNo,
                Email = order.Email,
                ItemCount = order.ItemCount,
                ItemId = order.ItemId
            });

            return await Task.FromResult(orderResponse);
        }
    }
}
