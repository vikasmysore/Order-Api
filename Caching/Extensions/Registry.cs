using Caching.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Caching.Extensions
{
    public static class Registry
    {
        public static IServiceCollection RegisterCaching(this IServiceCollection services, IConfiguration configuration)
        {
            var redisCacheSettings = configuration
            .GetSection("RedisCacheConfiguration")
            .Get<RedisCacheConfiguration>();

            services.AddSingleton(redisCacheSettings);


            return services;
        }
    }
}
