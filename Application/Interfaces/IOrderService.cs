using Domain.Models;

namespace Application.Interfaces
{
    public interface IOrderService
    {
        public Task<GetOrder> CreateOrder(Order order);

        public Task<IList<GetOrder>> GetAllOrders();

        public Task<GetOrder> GetOrderByOrderNo(string orderNo);

    }
}
