﻿namespace Infrastructure.Models
{
    public class OrderMessage
    {
        public required Guid OrderNo { get; set; }
        public required string ItemId { get; set; }
        public required int ItemCount { get; set; }
        public required string Email { get; set; }
    }
}
