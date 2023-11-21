using Libraries.Common.Events;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace IntegrationService.AgvProviderFactoryUs;

public class OrderQueuedEventConsumer : IConsumer<OrderQueuedEvent>
{
    private readonly ILogger<OrderQueuedEventConsumer> _logger;
    private readonly IBus _bus;
    public OrderQueuedEventConsumer(
        ILogger<OrderQueuedEventConsumer> logger,
        IBus bus)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    }

    public async Task Consume(ConsumeContext<OrderQueuedEvent> context)
    {
        var order = context.Message.Order;
        _logger.LogInformation("Recieved new order with id: '{id}' at '{occuredAt}'", order.Id, context.Message.OccuredAt);

        await InvokeProvider(order.Id).ConfigureAwait(false);
    }

    private async Task InvokeProvider(int orderId)
    {
        /*
         * Execute call towards external services
         */
        var externalProviderClient = new FakeExternalProvider.FakeExternalProvider(_bus);
        await externalProviderClient.DoWork(orderId).ConfigureAwait(false);
    }
}
