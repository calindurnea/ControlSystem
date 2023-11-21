using OrderService.Domain.Entities;

namespace Libraries.Common.Events;

public sealed record OrderCreatedEvent(Order Order)
{
    public DateTime OccuredAt { get; } = DateTime.UtcNow;
}
