using Infrastructure.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.AppDbContext;
using Persistence.Repository;
using Persistence.Repository.Interfaces;

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

            services.AddDbContext<OrderDbContext>(options =>
            {
                options.UseCosmos(connectionString: cosmosDbSettings.ConnectionString,
                    databaseName: cosmosDbSettings.DatabaseName, null);
            });

            return services;
        }
    }
}
