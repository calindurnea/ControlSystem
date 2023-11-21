using OrderService.Application.Dtos;
using OrderService.Application.Dtos.Requests;
using OrderService.Application.Dtos.Responses;
using OrderService.Domain.Entities;

namespace OrderService.Application.Services;

public interface IOrderService
{
    Task<OrderDto> CreateOrder(CreateOrderDto createOrder);
    Task<OrderDto> GetOrder(int id);
    Task<OrderStatus> GetOrderStatus(int id);
    Task<FilteredOrdersDto> GetFilteredOrders(OrdersFilterDto ordersFilter, long pageNumber, long pageSize);
}
