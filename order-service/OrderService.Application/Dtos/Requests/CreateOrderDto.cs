using OrderService.Domain.Entities;

namespace OrderService.Application.Dtos.Requests;

public sealed record CreateOrderDto(int FactoryId, OrderType OrderType, MoveCargoMetadata OrderMetadata);
