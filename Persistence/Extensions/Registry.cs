using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.AppDbContext;
using Persistence.Repository;
using Persistence.Repository.Interfaces;
using Persistence.Settings;
using StackExchange.Redis;

namespace Persistence.Extensions
{
    public static class Registry
    {
        public static IServiceCollection RegisterInMemoryAdapterExtensions(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<InMemoryDataStore>();

            services.AddScoped<IOrderRepository, OrderRepository>();

            var cosmosDbSettings = configuration
                .GetSection("CosmosDbConfiguration")
                .Get<CosmosDbSettings>();

            var useRedis = configuration.GetValue<bool>("UseRedis");
            if (useRedis)
            {
                services.Decorate<IOrderRepository, CachedOrderRepositoryDecorator>();
                var redisCacheSettings = configuration
                .GetSection("RedisCacheConfiguration")
                .Get<RedisCacheConfiguration>();

                services.AddSingleton(ConnectionMultiplexer.Connect(redisCacheSettings.ConnectionString!));
                services.AddStackExchangeRedisCache(options =>
                {
                    options.Configuration = redisCacheSettings.ConnectionString;
                    options.InstanceName = "OrderApi."; // Prefix for cache keys
                });
            }

            services.AddDbContext<OrderDbContext>(options =>
            {
                options.UseCosmos(connectionString: cosmosDbSettings.ConnectionString,
                    databaseName: cosmosDbSettings.DatabaseName, null);
            });

            return services;
        }
    }
}
