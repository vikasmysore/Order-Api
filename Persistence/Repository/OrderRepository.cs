using CrossCutting.ResultResponse;
using Microsoft.EntityFrameworkCore;
using Persistence.AppDbContext;
using Persistence.Entities;
using Persistence.Repository.Interfaces;
using Persistence.RequestModels;
using Persistence.ResponseModels;
using static Persistence.ResponseModels.CreateOrderResponse;
using static Persistence.ResponseModels.GetAllOrdersReponse;

namespace Persistence.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly OrderDbContext _context;

        public OrderRepository(OrderDbContext context)
        {
            _context = context;
        }

        public async Task<CreateOrderResponse> GetOrderByOrderNo(string id, string email)
        {
            Guid OrderId;
            if (Guid.TryParse(id, out OrderId))
            {
                var orderEntity = await _context.Orders.FirstOrDefaultAsync(o => o.OrderNo == OrderId && o.Email == email);

                if (orderEntity == null)
                {
                    return await Task.FromResult(new CreateOrderResponse(new ErrorResponse("Order Not Found")));
                }

                var response = new CreateOrderResponse(new CreateOrderResponseEntity()
                {
                    OrderNo = orderEntity.OrderNo,
                    Email = orderEntity.Email,
                    ItemCount = orderEntity.ItemCount,
                    ItemId = orderEntity.ItemId
                });

                return await Task.FromResult(response);
            }

            return await Task.FromResult(new CreateOrderResponse(new ErrorResponse("Order Not Found.")));
        }

        public async Task<GetAllOrdersReponse> GetAllOrders(string email)
        {
            var response = await _context.Orders.Where(o => o.Email == email).ToListAsync();

            if (response == null || response.Count == 0)
            {
                return await Task.FromResult(new GetAllOrdersReponse(new ErrorResponse("No orders to show.")));
            }

            var orderList = response.Select(r => new CreateOrderResponseEntity() { OrderNo = r.OrderNo, Email = r.Email, ItemCount = r.ItemCount, ItemId = r.ItemId });

            return await Task.FromResult(new GetAllOrdersReponse(new GetAllOrdersReponseEntity() { Orders = orderList }));
        }
        public async Task<CreateOrderResponse> CreateOrder(CreateOrderRequest orderRequest)
        {
            var order = new OrderEntity()
            {
                Email = orderRequest.Email,
                ItemCount = orderRequest.ItemCount,
                ItemId = orderRequest.ItemId,
                OrderNo = Guid.NewGuid(),
                PartitionKey = orderRequest.Email
            };

            if (string.IsNullOrEmpty(order.PartitionKey))
            {
                order.PartitionKey = order.Email; // or any other logic to derive the partition key
            }
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return await Task.FromResult(new CreateOrderResponse(new CreateOrderResponseEntity() { OrderNo = order.OrderNo, Email = order.Email, ItemCount = order.ItemCount, ItemId = order.ItemId }));
        }

        public async Task<CreateOrderResponse> UpdateOrderAsync(OrderEntity order)
        {
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
            return await Task.FromResult(new CreateOrderResponse(new CreateOrderResponseEntity() { OrderNo = order.OrderNo, Email = order.Email, ItemCount = order.ItemCount, ItemId = order.ItemId }));
        }

        public async Task<bool> DeleteOrderAsync(Guid id)
        {
            var orderEntity = await _context.Orders.FindAsync(id);

            if (orderEntity != null)
            {
                _context.Orders.Remove(orderEntity);
                await _context.SaveChangesAsync();

                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }
    }
}
