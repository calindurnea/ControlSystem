using Libraries.Common.Events;
using MassTransit;
using Microsoft.Extensions.Logging;
using OrderService.Domain.Entities;
using OrderService.OrderManager.Models;
using OrderService.OrderManager.Services;

namespace OrderService.OrderManager.EventConsumers;

public class OrderUpdatedEventConsumer : IConsumer<OrderUpdatedEvent>
{
    private readonly ILogger<OrderUpdatedEventConsumer> _logger;
    private readonly IBus _bus;
    private readonly IOrderLifecycleService _lifecycleService;

    public OrderUpdatedEventConsumer(
        ILogger<OrderUpdatedEventConsumer> logger,
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
        _logger.LogInformation("Received new order with id: '{id}' at '{occuredAt}'", order.Id, context.Message.OccuredAt);

        // Handle notifications of state update

        await _lifecycleService.CreateOrderLifecycle(new CreateOrderLifecycle(order.Id, OrderStatus.Queued, MachineStatus.Unknown)).ConfigureAwait(false);
        await _bus.Publish(context.Message).ConfigureAwait(false);
    }

    public async Task Consume(ConsumeContext<OrderUpdatedEvent> context)
    {
        _logger.LogInformation("Received order update with id: '{id}' at '{occuredAt}'", context.Message.OrderId, context.Message.OccuredAt);
        await _lifecycleService.CreateOrderLifecycle(new CreateOrderLifecycle(
            context.Message.OrderId,
            MapToOrderStatus(context.Message.MachineStatus),
            context.Message.MachineStatus,
            context.Message.MachineId)).ConfigureAwait(false);

    }

    private static OrderStatus MapToOrderStatus(MachineStatus machineStatus)
    {
        switch (machineStatus)
        {
            case MachineStatus.Busy:
            case MachineStatus.Unknown:
            case MachineStatus.Maintenance:
            case MachineStatus.Available:
                return OrderStatus.Queued;
            case MachineStatus.Started:
            case MachineStatus.InProgress:
                return OrderStatus.InProgress;
            case MachineStatus.Finished:
                return OrderStatus.Completed;
            default:
                throw new ArgumentOutOfRangeException(nameof(machineStatus), machineStatus, null);
        }
    }
}
