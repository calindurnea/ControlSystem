using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace OrderService.Persistence;

internal sealed class DbInitializer
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public DbInitializer(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    [SuppressMessage("Reliability", "CA2007:Consider calling ConfigureAwait on the awaited task")]
    internal async Task Initialize()
    {
        await using var scope = _serviceScopeFactory.CreateAsyncScope();
        var services = scope.ServiceProvider;

        var context = services.GetRequiredService<OrdersDbContext>();
        await context.Database.MigrateAsync().ConfigureAwait(false);
    }
}
