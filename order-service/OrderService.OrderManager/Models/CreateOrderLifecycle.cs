using OrderService.Domain.Entities;

namespace OrderService.OrderManager.Models;

public sealed record CreateOrderLifecycle(int OrderId, OrderStatus OrderStatus, MachineStatus MachineStatus, string? MachineId = null);
