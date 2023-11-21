using OrderService.Domain.Entities;

namespace OrderService.Persistence.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly IOrdersDbContext _ordersDbContext;

    public OrderRepository(IOrdersDbContext ordersDbContext)
    {
        _ordersDbContext = ordersDbContext ?? throw new ArgumentNullException(nameof(ordersDbContext));
    }

    public async Task<Order> Create(Order order)
    {
        var result = await _ordersDbContext.Orders.AddAsync(order).ConfigureAwait(false);
        await _ordersDbContext.SaveChangesAsync().ConfigureAwait(false);
        return result.Entity;
    }

    public async Task<Order?> GetById(int id)
    {
        var result = await _ordersDbContext.Orders.FindAsync(id).ConfigureAwait(false);
        return result;
    }
}
