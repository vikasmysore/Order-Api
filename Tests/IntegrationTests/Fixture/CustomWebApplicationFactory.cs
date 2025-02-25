using Api;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Infrastructure.Interfaces;
using Tests.IntegrationTests.Fakes;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.Extensions.Configuration;
using Microsoft.Azure.Cosmos;
using Microsoft.EntityFrameworkCore;
using Persistence.AppDbContext;

namespace Tests.IntegrationTests.Fixture
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        private readonly string _databaseName;
        private const string ContainerName = "Orders";

        public CustomWebApplicationFactory()
        {
            _databaseName = $"OrderTestDb"; // Unique DB per test run
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            // Configure app settings to use Cosmos Emulator
            builder.ConfigureAppConfiguration(config =>
            {
                config.AddInMemoryCollection(new Dictionary<string, string>
            {
                { "CosmosDb:Endpoint", "https://localhost:8081" },
                { "CosmosDb:Key", "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==" },
                { "CosmosDb:DatabaseName", _databaseName },
                { "CosmosDb:ContainerName", ContainerName }
            });
            });

            builder.ConfigureServices(services =>
            {

                var dbContextDescriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<OrderDbContext>)
            );
                if (dbContextDescriptor != null) services.Remove(dbContextDescriptor);

                // Create a new CosmosClient for setup/cleanup
                var cosmosClient = new CosmosClient(
                    "https://localhost:8081",
                    "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw=="
                );

                // Initialize database (EF Core will handle container creation)
                cosmosClient.CreateDatabaseIfNotExistsAsync(_databaseName).GetAwaiter().GetResult();
                var database = cosmosClient.GetDatabase(_databaseName);
                database.CreateContainerIfNotExistsAsync(ContainerName, "/PartitionKey").GetAwaiter().GetResult();

                // Configure EF Core to use Cosmos Emulator
                services.AddDbContext<OrderDbContext>(options =>
                {
                    options.UseCosmos(
                        "https://localhost:8081",
                        "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==",
                        _databaseName
                    );
                });

                // Ensure database schema is created
                using var scope = services.BuildServiceProvider().CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<OrderDbContext>();
                context.Database.EnsureCreatedAsync();

                // Remove real service bus client
                var descriptorServiceBus = services.SingleOrDefault(d => d.ServiceType == typeof(IAzureServiceBusClient));
                if (descriptorServiceBus != null)
                {
                    services.Remove(descriptorServiceBus);
                }

                // Add fake service bus client
                services.AddSingleton<IAzureServiceBusClient, FakeAzureServiceBusClient>();

                services.RemoveAll<IPolicyEvaluator>();
                services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
            });
        }
        public new async Task DisposeAsync()
        {
            await base.DisposeAsync();
        }
    }
}
