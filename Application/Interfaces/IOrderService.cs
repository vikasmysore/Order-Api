using Domain.Models;

namespace Application.Interfaces
{
    public interface IOrderService
    {
        public Task<GetOrder> CreateOrder(Order order);

        public Task<IList<GetOrder>> GetAllOrders(string email);

        public Task<GetOrder> GetOrderByOrderNo(string orderNo, string email);

    }
}
