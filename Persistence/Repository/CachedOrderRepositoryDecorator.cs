using CrossCutting.ResultResponse;
using Microsoft.Extensions.Caching.Distributed;
using Persistence.Entities;
using Persistence.Repository.Interfaces;
using Persistence.RequestModels;
using Persistence.ResponseModels;
using System.Text.Json;
using static Persistence.ResponseModels.CreateOrderResponse;
using static Persistence.ResponseModels.GetAllOrdersReponse;

namespace Persistence.Repository
{
    public class CachedOrderRepositoryDecorator : IOrderRepository
    {
        private readonly IOrderRepository _dbRepository;
        private readonly IDistributedCache _cache;
        private readonly TimeSpan _cacheExpiration = TimeSpan.FromMinutes(10);

        public CachedOrderRepositoryDecorator(IOrderRepository dbRepository, IDistributedCache cache)
        {
            _dbRepository = dbRepository;
            _cache = cache;
        }
        public async Task<CreateOrderResponse> CreateOrder(CreateOrderRequest order)
        {

            // If not in cache, get from database
            var createdOrder = await _dbRepository.CreateOrder(order);

            var cacheKey = $"order_{createdOrder.Content.OrderNo}";


            if (createdOrder != null && !createdOrder.IsError)
            {
                // Add to cache
                await _cache.SetStringAsync(
                    cacheKey,
                    JsonSerializer.Serialize(createdOrder.Content),
                    new DistributedCacheEntryOptions
                    {
                        SlidingExpiration = _cacheExpiration
                    });
            }
            else
            {
                return await Task.FromResult(new CreateOrderResponse(new ErrorResponse(createdOrder.ErrorResponse)));
            }

            var dbOrderResponse = new CreateOrderResponse(createdOrder.Content);

            return await Task.FromResult(dbOrderResponse);
        }

        public Task<bool> DeleteOrderAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<GetAllOrdersReponse> GetAllOrders(string email)
        {
            var cacheKey = $"order_{email}";
            var cachedOrders = await _cache.GetStringAsync(cacheKey);
            if (cachedOrders != null)
            {
                var cachedOrdersEntity = JsonSerializer.Deserialize<GetAllOrdersReponseEntity>(cachedOrders);

                return await Task.FromResult(new GetAllOrdersReponse(cachedOrdersEntity));
            }

            var orders = await _dbRepository.GetAllOrders(email);

            if (orders != null && !orders.IsError)
            {
                // Add to cache
                await _cache.SetStringAsync(
                    cacheKey,
                    JsonSerializer.Serialize(orders.Content),
                    new DistributedCacheEntryOptions
                    {
                        SlidingExpiration = _cacheExpiration
                    });
            }
            else
            {
                return await Task.FromResult(new GetAllOrdersReponse(new ErrorResponse(orders.ErrorResponse)));
            }

            var dbOrdersResponse = new GetAllOrdersReponse(orders.Content);

            return await Task.FromResult(dbOrdersResponse);
        }

        public async Task<CreateOrderResponse> GetOrderByOrderNo(string id, string email)
        {
            var cacheKey = $"order_{id}";

            // Try to get from cache
            var cachedOrder = await _cache.GetStringAsync(cacheKey);
            if (cachedOrder != null)
            {
                var cachedOrderEntity = JsonSerializer.Deserialize<CreateOrderResponseEntity>(cachedOrder);
                if (cachedOrderEntity.Email == email)
                {
                    var cacheOrderResponse = new CreateOrderResponse(cachedOrderEntity);

                    return await Task.FromResult(cacheOrderResponse);
                }
            }

            // If not in cache, get from database
            var order = await _dbRepository.GetOrderByOrderNo(id, email);

            if (order != null && !order.IsError)
            {
                // Add to cache
                await _cache.SetStringAsync(
                    cacheKey,
                    JsonSerializer.Serialize(order.Content),
                    new DistributedCacheEntryOptions
                    {
                        SlidingExpiration = _cacheExpiration
                    });
            }
            else
            {
                return await Task.FromResult(new CreateOrderResponse(new ErrorResponse(order.ErrorResponse)));
            }

            var dbOrderResponse = new CreateOrderResponse(order.Content);

            return await Task.FromResult(dbOrderResponse);
        }

        public Task<CreateOrderResponse> UpdateOrderAsync(OrderEntity order)
        {
            throw new NotImplementedException();
        }
    }
}
