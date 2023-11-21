using Microsoft.EntityFrameworkCore;
using OrderService.Domain.Entities;

namespace OrderService.Persistence;

public partial class OrdersDbContext : DbContext, IOrdersDbContext
{
    public OrdersDbContext(DbContextOptions<OrdersDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Order> Orders { get; set; }
    public virtual DbSet<OrderLifecycle> OrderLifecycles { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.HasDefaultSchema("orders");

        builder.Entity<Order>(entity =>
        {
            entity.ToTable("Order", "orders");

            entity.Property(e => e.FactoryId);
            entity.Property(e => e.OrderType)
                .HasConversion(
                    v => v.ToString(),
                    v => (OrderType)Enum.Parse(typeof(OrderType), v))
                .IsRequired();
            entity.Property(e => e.FactoryId).IsRequired();
        });

        builder.Entity<OrderLifecycle>(entity =>
        {
            entity.ToTable("OrderLifecycle", "orders");

            entity.Property(e => e.OrderId);
            entity.Property(e => e.MachineId);
            entity.Property(e => e.OrderStatus)
                .HasConversion(
                    v => v.ToString(),
                    v => (OrderStatus)Enum.Parse(typeof(OrderStatus), v))
                .IsRequired();
            entity.Property(e => e.MachineStatus)
                .HasConversion(
                    v => v.ToString(),
                    v => (MachineStatus)Enum.Parse(typeof(MachineStatus), v))
                .IsRequired();
        });
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker
            .Entries()
            .Where(e => e is { Entity: BaseEntity, State: EntityState.Added or EntityState.Modified });

        foreach (var entityEntry in entries)
        {
            ((BaseEntity)entityEntry.Entity).UpdatedDate = DateTime.UtcNow;

            if (entityEntry.State == EntityState.Added)
            {
                ((BaseEntity)entityEntry.Entity).CreatedDate = DateTime.UtcNow;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}
