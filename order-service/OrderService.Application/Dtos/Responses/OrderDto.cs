using OrderService.Application.Dtos.Requests;
using OrderService.Domain.Entities;

namespace OrderService.Application.Dtos.Responses;

public record OrderDto(int Id, int FactoryId, OrderType OrderType, MoveCargoMetadata? OrderMetadata);
