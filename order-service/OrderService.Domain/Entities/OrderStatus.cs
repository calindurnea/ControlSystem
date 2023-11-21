namespace OrderService.Domain.Entities;

public enum OrderStatus
{
    Created,
    Queued,
    InProgress,
    Completed,
    Failed,
    Canceled,
}
