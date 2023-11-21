using OrderService.Domain.Entities;

namespace OrderService.Persistence.Repositories;

public interface IOrderLifecycleRepository
{
    Task<OrderLifecycle> Create(OrderLifecycle orderLifecycle);
}
