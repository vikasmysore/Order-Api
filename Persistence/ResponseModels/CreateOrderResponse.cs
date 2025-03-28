using CrossCutting.ResultResponse;
using static Persistence.ResponseModels.CreateOrderResponse;

namespace Persistence.ResponseModels
{
    public class CreateOrderResponse : Result<CreateOrderResponseEntity>
    {
        public class CreateOrderResponseEntity
        {
            public Guid? OrderNo { get; set; }
            public string? ItemId { get; set; }
            public int? ItemCount { get; set; }
            public string? Email { get; set; }
        }

        public CreateOrderResponse(CreateOrderResponseEntity content) : base(content)
        {
        }

        public CreateOrderResponse(ErrorResponse errorResponse) : base(errorResponse)
        {
        }
    }
}
