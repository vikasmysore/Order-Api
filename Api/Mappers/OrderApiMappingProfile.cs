using Api.Dtos;
using AutoMapper;
using Domain.Models;

namespace Api.Mappers
{
    public class OrderApiMappingProfile : Profile
    {
        public OrderApiMappingProfile()
        {
            // Map from DTO to Domain Model
            CreateMap<OrderDto, Order>();
            CreateMap<GetOrder, GetOrderDto>();
        }
    }
}
