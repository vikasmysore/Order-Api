using Application.Mappers;
using Application.Interfaces;
using Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Extensions
{
    public static class Registry
    {
        public static IServiceCollection RegisterApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IUserInfoService, UserInfoService>();
            services.AddAutoMapper(cfg => { cfg.AddProfile<OrderApplicationMappingProfile>(); });

            return services;
        }
    }
}
