namespace OrderService.Domain.Entities;

public class BaseEntity
{
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
}
