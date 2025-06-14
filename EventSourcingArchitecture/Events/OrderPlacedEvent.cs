﻿using OnlineOrderSystem.Domain;

namespace OnlineOrderSystem.Events
{
    public class OrderPlacedEvent : BaseEvent
    {
        public Guid CustomerId { get; private set; }
        public List<OrderItem> Items { get; private set; }
        public decimal TotalAmount { get; private set; }
        public string ShippingAddress { get; private set; }

        // Constructor cho khi tạo event mới
        public OrderPlacedEvent(
            Guid aggregateId,
            Guid customerId,
            List<OrderItem> items,
            decimal totalAmount,
            string shippingAddress,
            int version)
            : base(aggregateId, version)
        {
            CustomerId = customerId;
            Items = items ?? throw new ArgumentNullException(nameof(items));
            TotalAmount = totalAmount;
            ShippingAddress = shippingAddress ?? throw new ArgumentNullException(nameof(shippingAddress));
        }

        // For deserialization
        private OrderPlacedEvent() : base() { }
    }
}