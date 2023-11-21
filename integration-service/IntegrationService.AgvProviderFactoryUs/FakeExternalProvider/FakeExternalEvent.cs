using OrderService.Domain.Entities;

namespace IntegrationService.AgvProviderFactoryUs.FakeExternalProvider;

public record FakeExternalEvent(int OrderId, MachineStatus MachineStatus, string MachineId);
