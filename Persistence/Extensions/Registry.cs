using Microsoft.Extensions.DependencyInjection;
namespace Persistence.Extensions
{
    public static class Registry
    {
        public static IServiceCollection RegisterInMemoryAdapterExtensions(this IServiceCollection services)
        {
            services.AddSingleton<InMemoryDataStore>();

            return services;
        }
    }
}
