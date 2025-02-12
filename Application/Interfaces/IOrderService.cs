namespace Application.Interfaces
{
    public interface IOrderService
    {
        public string CreateOrder(string orderId, string itemId, int itemCount, string email);
    }
}
