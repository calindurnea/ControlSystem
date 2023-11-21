using IntegrationService.AgvProviderFactoryUs.FakeExternalProvider;
using Libraries.Common.Events;
using MassTransit;

namespace IntegrationService.AgvProviderFactoryUs;

public class AgvProviderUsEventConsumer : IConsumer<FakeExternalEvent>
{
    private readonly IBus _bus;

    public AgvProviderUsEventConsumer(IBus bus)
    {
        _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    }

    public async Task Consume(ConsumeContext<FakeExternalEvent> context)
    {
        await _bus.Publish(new OrderUpdatedEvent(context.Message.OrderId, context.Message.MachineStatus, context.Message.MachineId)).ConfigureAwait(false);
    }
}
