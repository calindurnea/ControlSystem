namespace OrderService.Domain.Entities;

public sealed class Order : BaseEntity
{
    public int Id { get; set; }
    public int FactoryId { get; init; }
    public OrderType OrderType { get; init; }
    public string OrderMetadata { get; set; }
}
