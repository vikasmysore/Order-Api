using Api.Dtos;
using AutoMapper;
using Domain.Models;

namespace Api.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Map from DTO to Domain Model
            CreateMap<OrderDto, Order>();
        }
    }
}
