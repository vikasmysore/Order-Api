using Persistence.Entities;
using Persistence.ResponseModels;
using Persistence.RequestModels;

namespace Persistence.Repository.Interfaces
{
    public interface IOrderRepository
    {
        Task<CreateOrderResponse> GetOrderByOrderNo(string id, string email);
        Task<GetAllOrdersReponse> GetAllOrders(string email);
        Task<CreateOrderResponse> CreateOrder(CreateOrderRequest order);
        Task<CreateOrderResponse> UpdateOrderAsync(OrderEntity order);
        Task<bool> DeleteOrderAsync(Guid id);
    }
}
