using System.Reflection;
using FluentValidation;
using FluentValidation.AspNetCore;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using OrderService.Application.ExceptionHandlers;
using OrderService.Application.Services;
using OrderService.OrderManager.EventConsumers;

namespace OrderService.Application.ProgramExtensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IOrderService, Services.OrderService>();
        return services;
    }

    public static IServiceCollection AddValidators(this IServiceCollection services)
    {
        services.AddFluentValidationAutoValidation();
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddExceptionHandler<DefaultExceptionHandler>();
        return services;
    }

    public static IServiceCollection AddEventPublisher(this IServiceCollection services, Action<RabbitMqOptions> rabbitMqConfiguration)
    {
        var rabbitMqOptions = new RabbitMqOptions();
        rabbitMqConfiguration(rabbitMqOptions);

        services.AddMassTransit(x =>
        {
            x.AddConsumers(
                typeof(OrderCreatedEventConsumer),
                typeof(OrderUpdatedEventConsumer));

            x.SetKebabCaseEndpointNameFormatter();
            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(rabbitMqOptions.Hostname, h =>
                {
                    h.Username(rabbitMqOptions.Username);
                    h.Password(rabbitMqOptions.Password);
                });
                cfg.ConfigureEndpoints(context);
            });
        });
        return services;
    }
}
