using Api.Dtos;
using Tests.IntegrationTests.Fixture;
using System.Net.Http.Json;
using Xunit;
using System.Text.Json;

namespace Tests.IntegrationTests.Tests
{
    public class OrderTests : BaseTest
    {
        private Guid OrderId;
        public OrderTests(CustomWebApplicationFactory factory) : base(factory)
        {
        }

        [Fact]
        public async Task CreateOrder_Should_StoreOrder_And_SendMessageToQueue()
        {
            var order = new OrderDto { ItemId = "Laptop", ItemCount = 1, Email = "Test@Test.com" };

            var response = await _client.PostAsJsonAsync("/Order/", order);

            response.EnsureSuccessStatusCode();
            Assert.NotNull(response);

            var resposneContent = await response.Content.ReadAsStringAsync();

            var orderObject = JsonSerializer.Deserialize<GetOrderDto>(resposneContent);
            OrderId = orderObject.OrderNo;
        }

        [Fact]
        public async Task GetOrder_Should_GetOrder_By_OrderNo()
        {
            // Act
            var response = await _client.GetAsync($"/Order/GetOrderByOrderNo/{OrderId}");

            // Assert
            response.EnsureSuccessStatusCode();
            var resposneContent = await response.Content.ReadAsStringAsync();

            var orderObject = JsonSerializer.Deserialize<GetOrderDto>(resposneContent);
            Assert.Equal(OrderId, orderObject.OrderNo);
        }
    }
}
