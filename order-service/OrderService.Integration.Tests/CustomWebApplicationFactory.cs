using Microsoft.AspNetCore.Mvc.Testing;
using Testcontainers.PostgreSql;
using Testcontainers.RabbitMq;

namespace OrderService.Integration.Tests;

public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
    public RabbitMqContainer RabbitMqContainer;
    public CustomWebApplicationFactory()
    {
        RabbitMqContainer = new RabbitMqBuilder()
            .WithImage("rabbitmq:management")
            .WithUsername("guest")
            .WithPassword("guest")
            .WithCleanUp(true)
            .WithPortBinding(5672, 5672)
            .WithPortBinding(15672, 15672)
            .WithExposedPort(15672)
            .Build();

        var postgreSqlContainer = new PostgreSqlBuilder()
            .WithDatabase("orders")
            .WithPassword("orders-admin")
            .WithUsername("orders-admin")
            .WithImage("postgres:latest")
            .WithCleanUp(true)
            .WithPortBinding(5432)
            .Build();

        RabbitMqContainer.StartAsync().Wait();
        postgreSqlContainer.StartAsync().Wait();
    }
}
