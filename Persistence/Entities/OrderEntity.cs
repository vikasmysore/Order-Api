using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Persistence.Entities
{
    public class OrderEntity
    {
        [Key]
        [JsonProperty("id")]
        public required Guid OrderNo { get; set; }
        public required string PartitionKey { get; set; }
        public required string ItemId { get; set; }
        public required int ItemCount { get; set; }
        public required string Email { get; set; }
    }
}
