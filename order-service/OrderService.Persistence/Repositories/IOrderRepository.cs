using OrderService.Domain.Entities;

namespace OrderService.Persistence.Repositories;

public interface IOrderRepository
{
    Task<Order> Create(Order order);
    Task<Order?> GetById(int id);
}
