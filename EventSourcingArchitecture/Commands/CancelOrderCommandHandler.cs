﻿using OnlineOrderSystem.Domain;
using OnlineOrderSystem.EventBus;
using OnlineOrderSystem.EventStore;

namespace OnlineOrderSystem.Commands
{
    public class CancelOrderCommandHandler
    {
        private readonly IEventStore _eventStore;
        private readonly IEventBus _eventBus;
        private readonly ILogger<CancelOrderCommandHandler> _logger;

        public CancelOrderCommandHandler(
            IEventStore eventStore,
            IEventBus eventBus,
            ILogger<CancelOrderCommandHandler> logger)
        {
            _eventStore = eventStore;
            _eventBus = eventBus;
            _logger = logger;
        }

        public async Task Handle(CancelOrderCommand command)
        {
            _logger.LogInformation("Handling CancelOrderCommand for order {OrderId}", command.OrderId);

            // Load order từ events
            var events = await _eventStore.GetEventsAsync(command.OrderId);
            var order = Order.FromEvents(events);

            // Business logic - cancel order
            order.Cancel(command.Reason);

            // Lấy new events
            var newEvents = order.GetUncommittedEvents();

            // Lưu events
            await _eventStore.SaveEventsAsync(order.Id, newEvents, order.Version - newEvents.Count());

            // Publish events
            await _eventBus.PublishAsync(newEvents);

            order.ClearUncommittedEvents();

            _logger.LogInformation("Order {OrderId} cancelled successfully", command.OrderId);
        }
    }
}