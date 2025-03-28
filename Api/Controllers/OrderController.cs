using Api.Dtos;
using Application.Interfaces;
using AutoMapper;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class OrderController(IOrderService orderService, IMapper mapper, IUserInfoService userInfoService) : ControllerBase
    {
        [HttpPost]
        public async Task<GetOrderDto> CreateOrder([FromBody] OrderDto orderDto)
        {
            try
            {
                var order = mapper.Map<Order>(orderDto);

                var response = await orderService.CreateOrder(order);

                return mapper.Map<GetOrderDto>(response);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet("GetAllOrders")]
        public async Task<IEnumerable<GetOrderDto>> GetAllOrders()
        {
            try
            {
                var email = userInfoService.GetEmail();

                var response = await orderService.GetAllOrders(email);

                return response.Select(r => mapper.Map<GetOrderDto>(r));
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        [HttpGet(("GetOrderByOrderNo/{orderNo}"))]
        public async Task<GetOrderDto> GetOrderByOrderNo(string orderNo)
        {
            try
            {
                var email = userInfoService.GetEmail();
                var response = await orderService.GetOrderByOrderNo(orderNo, email);

                return mapper.Map<GetOrderDto>(response);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
