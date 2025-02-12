using Infrastructure.Clients;
using Infrastructure.Interfaces;
using Infrastructure.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Extensions
{
    public static class Registry
    {
        public static IServiceCollection RegisterInfrastructureClients(this IServiceCollection services, IConfiguration configuration)
        {
            var serviceBusSettings = configuration
            .GetSection("ServiceBusConfiguration")
            .Get<ServiceBusSettings>();

            services.AddSingleton(serviceBusSettings);

            services.AddSingleton<IAzureServiceBusClient, AzureServiceBusClient>();

            return services;
        }
    }
}
