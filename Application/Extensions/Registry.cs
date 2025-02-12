using Application.Mappers;
using Application.Interfaces;
using Application.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Application.Settings;

namespace Application.Extensions
{
    public static class Registry
    {
        public static IServiceCollection RegisterApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            var serviceBusSettings = configuration
            .GetSection("ServiceBusConfiguration")
            .Get<ServiceBusSettings>();

            services.AddSingleton(serviceBusSettings);

            services.AddScoped<IOrderService, OrderService>();
            services.AddAutoMapper(cfg => { cfg.AddProfile<OrderApplicationMappingProfile>(); });

            return services;
        }
    }
}
