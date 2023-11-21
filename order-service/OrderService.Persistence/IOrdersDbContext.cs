using Microsoft.EntityFrameworkCore;
using OrderService.Domain.Entities;

namespace OrderService.Persistence;

public interface IOrdersDbContext
{
    DbSet<Order> Orders { get; }
    DbSet<OrderLifecycle> OrderLifecycles { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
