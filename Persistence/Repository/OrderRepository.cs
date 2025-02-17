using Microsoft.EntityFrameworkCore;
using Persistence.AppDbContext;
using Persistence.Entities;
using Persistence.Repository.Interfaces;
using Persistence.RequestModels;
using Persistence.ResponseModels;

namespace Persistence.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly OrderDbContext _context;

        public OrderRepository(OrderDbContext context)
        {
            _context = context;
        }

        public async Task<CreateOrderResponse> GetOrderByOrderNo(string id)
        {
            var orderEntity = await _context.Orders.FindAsync(Guid.Parse(id));
            CreateOrderResponse orderResponse = new CreateOrderResponse();

            if (orderEntity == null)
            {
                return orderResponse;
            }

            orderResponse.OrderNo = orderEntity.OrderNo;
            orderResponse.Email = orderEntity.Email;
            orderResponse.ItemCount = orderEntity.ItemCount;
            orderResponse.ItemId = orderEntity.ItemId;
            return orderResponse;
        }

        public async Task<IEnumerable<CreateOrderResponse>> GetAllOrders()
        {
            var response = await _context.Orders.ToListAsync();

            return await Task.FromResult(response.Select(r => new CreateOrderResponse { OrderNo = r.OrderNo, Email = r.Email, ItemCount = r.ItemCount, ItemId = r.ItemId }));
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

            return new CreateOrderResponse { OrderNo = order.OrderNo, Email = order.Email, ItemCount = order.ItemCount, ItemId = order.ItemId };
        }

        public async Task<CreateOrderResponse> UpdateOrderAsync(OrderEntity order)
        {
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
            return new CreateOrderResponse { OrderNo = order.OrderNo, Email = order.Email, ItemCount = order.ItemCount, ItemId = order.ItemId };
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
