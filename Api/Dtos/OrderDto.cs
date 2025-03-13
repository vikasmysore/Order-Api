using System.Text.Json.Serialization;

namespace Api.Dtos
{
    public class OrderDto
    {
        [JsonPropertyName("itemId")]
        public string ItemId { get; set; }

        [JsonPropertyName("itemCount")]
        public int ItemCount { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }
    }
}
