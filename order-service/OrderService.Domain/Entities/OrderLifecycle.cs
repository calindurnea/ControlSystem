namespace OrderService.Domain.Entities;

public sealed class OrderLifecycle : BaseEntity
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public OrderStatus OrderStatus { get; set; }
    public MachineStatus MachineStatus { get; set; }
    public string? MachineId { get; set; }
}
