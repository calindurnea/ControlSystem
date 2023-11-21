using OrderService.OrderManager.Models;

namespace OrderService.OrderManager.Services;

public interface IOrderLifecycleService
{
    Task CreateOrderLifecycle(CreateOrderLifecycle createOrderLifecycle);
}
