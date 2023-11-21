using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace IntegrationService.AgvProviderFactoryUs.ProgramExtensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAgvProviderFactoryUsIntegration(this IServiceCollection services, Action<RabbitMqOptions> rabbitMqConfiguration)
    {
        var rabbitMqOptions = new RabbitMqOptions();
        rabbitMqConfiguration(rabbitMqOptions);

        services.AddMassTransit(x =>
        {
            x.SetKebabCaseEndpointNameFormatter();
            x.AddConsumers(typeof(OrderQueuedEventConsumer), typeof(AgvProviderUsEventConsumer));

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
