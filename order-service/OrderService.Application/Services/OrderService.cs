using Libraries.Common.Events;
using MassTransit;
using Newtonsoft.Json;
using OrderService.Application.Dtos;
using OrderService.Application.Dtos.Requests;
using OrderService.Application.Dtos.Responses;
using OrderService.Application.Exceptions;
using OrderService.Domain.Entities;
using OrderService.Persistence.Repositories;

namespace OrderService.Application.Services;

internal sealed class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IPublishEndpoint _publishEndpoint;

    public OrderService(
        IOrderRepository orderRepository,
        IPublishEndpoint publishEndpoint)
    {
        _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        _publishEndpoint = publishEndpoint ?? throw new ArgumentNullException(nameof(publishEndpoint));
    }

    public async Task<OrderDto> CreateOrder(CreateOrderDto createOrder)
    {
        var result = await _orderRepository.Create(new Order { FactoryId = createOrder.FactoryId, OrderType = createOrder.OrderType, OrderMetadata = JsonConvert.SerializeObject(createOrder.OrderMetadata), })
            .ConfigureAwait(false);

        await _publishEndpoint.Publish(new OrderCreatedEvent(result)).ConfigureAwait(false);

        return MapToDto(result);
    }

    public async Task<OrderDto> GetOrder(int id)
    {
        var result = await _orderRepository.GetById(id).ConfigureAwait(false);

        if (result is null)
        {
            throw new NotFoundException(id);
        }

        return MapToDto(result);
    }

    public Task<OrderStatus> GetOrderStatus(int id)
    {
        throw new NotImplementedException();
    }

    public Task<FilteredOrdersDto> GetFilteredOrders(OrdersFilterDto ordersFilter, long pageNumber, long pageSize)
    {
        throw new NotImplementedException();
    }

    private static OrderDto MapToDto(Order order)
    {
        return new OrderDto(
            order.Id,
            order.FactoryId,
            order.OrderType,
            JsonConvert.DeserializeObject<MoveCargoMetadata>(order.OrderMetadata));
    }
}
