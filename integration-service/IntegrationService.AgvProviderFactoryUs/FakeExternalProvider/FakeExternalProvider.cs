using MassTransit;
using OrderService.Domain.Entities;

namespace IntegrationService.AgvProviderFactoryUs.FakeExternalProvider;

public class FakeExternalProvider
{
    private readonly IBus _bus;

    public FakeExternalProvider(IBus bus)
    {
        _bus = bus;
    }

    public async Task DoWork(int orderId)
    {
        await _bus.Publish(new FakeExternalEvent(orderId, MachineStatus.InProgress, "MachineUs1")).ConfigureAwait(false);

        await Task.Delay(new Random().Next(1000, 10000)).ConfigureAwait(false);

        await _bus.Publish(new FakeExternalEvent(orderId, MachineStatus.Finished, "MachineUs1")).ConfigureAwait(false);
    }
}
