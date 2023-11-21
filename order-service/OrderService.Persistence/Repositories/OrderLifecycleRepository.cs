using OrderService.Domain.Entities;

namespace OrderService.Persistence.Repositories;

public class OrderLifecycleRepository : IOrderLifecycleRepository
{
    private readonly IOrdersDbContext _ordersDbContext;

    public OrderLifecycleRepository(IOrdersDbContext ordersDbContext)
    {
        _ordersDbContext = ordersDbContext ?? throw new ArgumentNullException(nameof(ordersDbContext));
    }

    public async Task<OrderLifecycle> Create(OrderLifecycle orderLifecycle)
    {
        var result = await _ordersDbContext.OrderLifecycles.AddAsync(orderLifecycle).ConfigureAwait(false);
        await _ordersDbContext.SaveChangesAsync().ConfigureAwait(false);
        return result.Entity;
    }
}
