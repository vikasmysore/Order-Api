﻿namespace Domain.Models
{
    public class Order
    {
        public required string ItemId { get; set; }
        public required int ItemCount { get; set; }
        public required string Email { get; set; }
    }
}
