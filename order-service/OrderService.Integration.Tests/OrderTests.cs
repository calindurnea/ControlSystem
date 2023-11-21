using System.Net;
using System.Text;
using AutoFixture;
using OrderService.Application.Dtos.Requests;
using OrderService.Application.Dtos.Responses;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Xunit;

namespace OrderService.Integration.Tests;

public class OrderTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _httpClient;
    private readonly Fixture _fixture = new();
    private readonly CustomWebApplicationFactory<Program> _factory;

    public OrderTests(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _httpClient = _factory.CreateClient();
    }

    [Fact]
    public async Task CreateOrderStartsLifecycleFlow()
    {
        // Connect to rabbit mq and establish queue
        var connectionFactory = new ConnectionFactory { Uri = new Uri(_factory.RabbitMqContainer.GetConnectionString()) };
        using var connection = connectionFactory.CreateConnection();
        var (waitHandle, actualMessage) = ConnectToRabbitMqQueue(connection, out var channel, out var consumer);
        channel.BasicConsume("order-queued-event", true, consumer);

        // Create and validate order creation
        var order = _fixture.Create<CreateOrderDto>();
        var createOrderResponse = await _httpClient.PostAsJsonAsync("/api/Orders/", order).ConfigureAwait(false);
        Assert.Equal(HttpStatusCode.Created, createOrderResponse.StatusCode);

        var orderId = createOrderResponse.Headers.Location?.ToString().Split("/").Last();
        Assert.NotNull(orderId);
        Assert.True(int.TryParse(orderId, out var parsedOrderId));

        var createdOrderResponse = await _httpClient.GetAsync($"/api/Orders/{parsedOrderId}").ConfigureAwait(false);
        Assert.Equal(HttpStatusCode.OK, createdOrderResponse.StatusCode);

        var currentOrderResponseBody = await createdOrderResponse.Content.ReadFromJsonAsync<OrderDto>().ConfigureAwait(false);
        Assert.NotNull(currentOrderResponseBody);
        Assert.Equal(parsedOrderId, currentOrderResponseBody.Id);
        Assert.Equal(order.FactoryId, currentOrderResponseBody.FactoryId);
        Assert.Equal(order.OrderType, currentOrderResponseBody.OrderType);

        // Consume queued event
        waitHandle.WaitOne(TimeSpan.FromSeconds(1));

        Assert.NotNull(actualMessage);
    }

    private static (EventWaitHandle waitHandle, string? actualMessage) ConnectToRabbitMqQueue(IConnection connection, out IModel channel, out EventingBasicConsumer consumer)
    {
        string? actualMessage = null;

        EventWaitHandle waitHandle = new ManualResetEvent(false);

        channel = connection.CreateModel();
        channel.QueueDeclare("order-queued-event", true, false, false, null);
        consumer = new EventingBasicConsumer(channel);
        consumer.Received += (_, eventArgs) =>
        {
            actualMessage = Encoding.Default.GetString(eventArgs.Body.ToArray());
            waitHandle.Set();
        };
        return (waitHandle, actualMessage);
    }
}
