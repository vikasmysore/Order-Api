using Infrastructure.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Persistence.AppDbContext;
using Tests.IntegrationTests.Fakes;
using Tests.IntegrationTests.Fixture;
using Xunit;

namespace Tests.IntegrationTests
{
    public class BaseTest : IClassFixture<CustomWebApplicationFactory>
    {
        protected readonly HttpClient _client;
        protected readonly FakeAzureServiceBusClient _fakeServiceBusClient;
        protected readonly OrderDbContext _context;

        public BaseTest(CustomWebApplicationFactory factory)
        {
            _fakeServiceBusClient = factory.Services.GetRequiredService<IAzureServiceBusClient>() as FakeAzureServiceBusClient;
            _client = factory.CreateClient();
            using (var scope = factory.Services.CreateScope())
            {
                _context = scope.ServiceProvider.GetRequiredService<OrderDbContext>();
            }
        }

    }
}
