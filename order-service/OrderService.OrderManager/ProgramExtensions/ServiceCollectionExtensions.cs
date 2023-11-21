using Microsoft.Extensions.DependencyInjection;
using OrderService.OrderManager.Services;

namespace OrderService.OrderManager.ProgramExtensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddOrderManagerServices(this IServiceCollection services)
    {
        services.AddScoped<IOrderLifecycleService, OrderLifecycleService>();
        return services;
    }
}
