namespace Persistence.RequestModels
{
    public class CreateOrderRequest
    {
        public required string ItemId { get; set; }
        public required int ItemCount { get; set; }
        public required string Email { get; set; }
    }
}
