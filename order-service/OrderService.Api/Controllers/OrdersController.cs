using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using OrderService.Application.Dtos.Requests;
using OrderService.Application.Dtos.Responses;
using OrderService.Application.Services;
using CreateOrderDto = OrderService.Application.Dtos.Requests.CreateOrderDto;

namespace OrderService.Api.Controllers;

/// <inheritdoc />
[ApiController]
[Route("api/[controller]")]
public class OrdersController : Controller
{
    private readonly IOrderService _orderService;

    /// <summary>
    /// Provides functionality for managing orders
    /// </summary>
    /// <param name="orderService"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public OrdersController(IOrderService orderService)
    {
        _orderService = orderService ?? throw new ArgumentNullException(nameof(orderService));
    }

    /// <summary>
    /// Returns the state of the order with the specified id if any is found
    /// </summary>
    /// <param name="id">Id of the order to return the status of</param>
    /// <returns>The order or not found</returns>
    [HttpGet("{id:int}/status", Name = nameof(GetOrderStatusForId))]
    [ProducesResponseType(typeof(OrderDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetOrderStatusForId([FromRoute, BindRequired] int id)
    {
        var orderStatus = await _orderService.GetOrderStatus(id).ConfigureAwait(false);
        return Ok(orderStatus);
    }

    /// <summary>
    /// Returns the order with the specified id if any is found
    /// </summary>
    /// <param name="id">Id of the order to return</param>
    /// <returns>The order or not found</returns>
    [HttpGet("{id:int}", Name = nameof(GetOrderWithId))]
    [ProducesResponseType(typeof(OrderDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetOrderWithId([FromRoute, BindRequired] int id)
    {
        var order = await _orderService.GetOrder(id).ConfigureAwait(false);
        return Ok(order);
    }

    /// <summary>
    /// Returns the orders based on the filters
    /// </summary>
    /// <param name="ordersFilterFilterDto">Filters to apply to the result</param>
    /// <param name="pageNumber">Page number to return. Defaults to 1</param>
    /// <param name="pageSize">Amount of items per result page. Defaults to 10</param>
    /// <returns>The order or not found</returns>
    [HttpPost("filtered", Name = nameof(GetFilteredOrders))]
    [ProducesResponseType(typeof(PaginatedResponse<OrderDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetFilteredOrders(
        [FromBody, BindRequired] OrdersFilterDto ordersFilterFilterDto,
        [FromRoute] long pageNumber = 1,
        [FromRoute] long pageSize = 10)
    {
        var filteredOrders = await _orderService.GetFilteredOrders(ordersFilterFilterDto, pageNumber, pageSize).ConfigureAwait(false);
        return Ok(filteredOrders);
    }

    /// <summary>
    /// Creates a new order.
    /// </summary>
    /// <param name="createOrder">Request for creating a new order</param>
    [HttpPost(Name = nameof(CreateOrder))]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateOrder([FromBody, BindRequired] CreateOrderDto createOrder)
    {
        var orderDto = await _orderService.CreateOrder(createOrder).ConfigureAwait(false);
        return CreatedAtAction(nameof(GetOrderWithId), new { id = orderDto.Id }, new { id = orderDto.Id });
    }
}
