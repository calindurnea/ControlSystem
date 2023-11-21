namespace OrderService.Domain.Entities;

public enum MachineStatus
{
    Unknown,
    Available,
    Busy,
    Maintenance,
    Started,
    InProgress,
    Finished
}
