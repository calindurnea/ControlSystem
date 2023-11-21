using OrderService.Application.Dtos.Responses;

namespace OrderService.Application.Dtos;

public sealed record FilteredOrdersDto(IEnumerable<OrderDto> OrderDtos, int TotalOrders);
