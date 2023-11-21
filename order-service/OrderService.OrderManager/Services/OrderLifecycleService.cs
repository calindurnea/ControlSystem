using OrderService.Domain.Entities;
using OrderService.OrderManager.Models;
using OrderService.Persistence.Repositories;

namespace OrderService.OrderManager.Services;

internal sealed class OrderLifecycleService : IOrderLifecycleService
{
    private readonly IOrderLifecycleRepository _orderLifecycleRepository;

    public OrderLifecycleService(IOrderLifecycleRepository orderLifecycleRepository)
    {
        _orderLifecycleRepository = orderLifecycleRepository ?? throw new ArgumentNullException(nameof(orderLifecycleRepository));
    }

    public async Task CreateOrderLifecycle(CreateOrderLifecycle createOrderLifecycle)
    {
        await _orderLifecycleRepository.Create(new OrderLifecycle
        {
            OrderId = createOrderLifecycle.OrderId,
            OrderStatus = createOrderLifecycle.OrderStatus,
            MachineId = createOrderLifecycle.MachineId,
            MachineStatus = createOrderLifecycle.MachineStatus
        })
            .ConfigureAwait(false);
    }
}
