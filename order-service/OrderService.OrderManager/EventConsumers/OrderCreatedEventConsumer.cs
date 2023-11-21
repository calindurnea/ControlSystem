using Libraries.Common.Events;
using MassTransit;
using Microsoft.Extensions.Logging;
using OrderService.Domain.Entities;
using OrderService.OrderManager.Models;
using OrderService.OrderManager.Services;

namespace OrderService.OrderManager.EventConsumers;

public class OrderCreatedEventConsumer : IConsumer<OrderCreatedEvent>
{
    private readonly ILogger<OrderCreatedEventConsumer> _logger;
    private readonly IBus _bus;
    private readonly IOrderLifecycleService _lifecycleService;

    public OrderCreatedEventConsumer(
        ILogger<OrderCreatedEventConsumer> logger,
        IBus bus,
        IOrderLifecycleService lifecycleService)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _bus = bus ?? throw new ArgumentNullException(nameof(bus));
        _lifecycleService = lifecycleService ?? throw new ArgumentNullException(nameof(lifecycleService));
    }

    public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
    {
        var order = context.Message.Order;
        _logger.LogInformation("Recieved new order with id: '{id}' at '{occuredAt}'", order.Id, context.Message.OccuredAt);

        // Handle possible mappings to specific integration req
        // Handle notifications of state update

        await _lifecycleService.CreateOrderLifecycle(new CreateOrderLifecycle(order.Id, OrderStatus.Queued, MachineStatus.Unknown)).ConfigureAwait(false);
        await _bus.Publish(new OrderQueuedEvent(context.Message.Order)).ConfigureAwait(false);
    }
}
