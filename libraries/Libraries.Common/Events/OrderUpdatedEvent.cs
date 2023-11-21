using OrderService.Domain.Entities;

namespace Libraries.Common.Events;

public sealed record OrderUpdatedEvent(int OrderId, MachineStatus MachineStatus, string? MachineId)
{
    public DateTime OccuredAt { get; } = DateTime.UtcNow;
}
