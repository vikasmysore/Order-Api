using CrossCutting.ResultResponse;
using static Persistence.ResponseModels.CreateOrderResponse;
using static Persistence.ResponseModels.GetAllOrdersReponse;

namespace Persistence.ResponseModels
{
    public class GetAllOrdersReponse : Result<GetAllOrdersReponseEntity>
    {
        public class GetAllOrdersReponseEntity
        {
            public IEnumerable<CreateOrderResponseEntity> Orders { get; set; }
        }

        public GetAllOrdersReponse(GetAllOrdersReponseEntity content) : base(content)
        {
        }

        public GetAllOrdersReponse(ErrorResponse errorResponse) : base(errorResponse)
        {
        }
    }
}
