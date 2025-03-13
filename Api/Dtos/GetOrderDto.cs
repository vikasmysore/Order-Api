using System.Text.Json.Serialization;

namespace Api.Dtos
{
    public class GetOrderDto : OrderDto
    {
        [JsonPropertyName("orderNo")]
        public Guid OrderNo { get; set; }
    }
}
