using System.Reflection;
using MassTransit;

namespace Messaging.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection ConfigureMassTransit(this IServiceCollection services, Assembly consumerAssembly)
    {
        services.AddMassTransit(busConfigurator =>
        {
            busConfigurator.AddConsumers(consumerAssembly);
            busConfigurator.UsingInMemory();
        });

        return services;
    }
}