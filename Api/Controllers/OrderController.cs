using Api.Dtos;
using Application.Interfaces;
using AutoMapper;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController(IOrderService orderService, IMapper mapper) : ControllerBase
    {
        [HttpPost]
        public async Task<GetOrderDto> CreateOrder([FromBody] OrderDto orderDto)
        {
            var order = mapper.Map<Order>(orderDto);

            var response = await orderService.CreateOrder(order);

            return mapper.Map<GetOrderDto>(response);

        }

        [HttpGet("GetAllOrders")]
        public async Task<IEnumerable<GetOrderDto>> GetAllOrders()
        {

            var response = await orderService.GetAllOrders();

            return response.Select(r => mapper.Map<GetOrderDto>(r));

        }

        [HttpGet(("GetOrderByOrderNo/{orderNo}"))]
        public async Task<GetOrderDto> GetOrderByOrderNo(string orderNo)
        {

            var response = await orderService.GetOrderByOrderNo(orderNo);

            return mapper.Map<GetOrderDto>(response);

        }
    }
}
