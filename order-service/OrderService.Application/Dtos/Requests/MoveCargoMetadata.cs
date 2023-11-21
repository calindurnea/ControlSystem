namespace OrderService.Application.Dtos.Requests;

public sealed record MoveCargoMetadata(string StartLocation, string EndLocation, string PreferredMachine, string CargoType, int[] CargoIds);
