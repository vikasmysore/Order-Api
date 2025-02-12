namespace Persistence.ResponseModels
{
    public class CreateOrderResponse
    {
        public Guid? OrderNo { get; set; }
        public string? ItemId { get; set; }
        public int? ItemCount { get; set; }
        public string? Email { get; set; }
    }
}
