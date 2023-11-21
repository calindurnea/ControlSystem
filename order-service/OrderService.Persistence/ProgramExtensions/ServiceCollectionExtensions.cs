using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderService.Persistence.Repositories;

namespace OrderService.Persistence.ProgramExtensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<OrdersDbContext>(options =>
        {
            options.ConfigureWarnings(builder => builder.Ignore(CoreEventId.MultipleNavigationProperties));
            options.UseNpgsql(
                configuration.GetConnectionString("OrdersContext"),
                x => x.MigrationsHistoryTable(HistoryRepository.DefaultTableName, "orders"));
        });
        services.AddScoped<IOrdersDbContext, OrdersDbContext>();

        services.AddSingleton<DbInitializer>();

        return services;
    }

    public static IApplicationBuilder InitializeDatabase(this IApplicationBuilder app)
    {
        ArgumentNullException.ThrowIfNull(app, nameof(app));

        var dbInitializer = app.ApplicationServices.GetRequiredService<DbInitializer>();

        var initializationTask = Task.Run(dbInitializer.Initialize);
        initializationTask.Wait();

        Console.WriteLine($"Initialization completed successfully: {initializationTask.IsCompletedSuccessfully}");

        return app;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IOrderLifecycleRepository, OrderLifecycleRepository>();
        return services;
    }
}
