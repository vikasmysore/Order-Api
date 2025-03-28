using AutoMapper;
using Domain.Models;
using Infrastructure.Models;
using Persistence.RequestModels;
using static Persistence.ResponseModels.CreateOrderResponse;

namespace Application.Mappers
{
    public class OrderApplicationMappingProfile : Profile
    {
        public OrderApplicationMappingProfile()
        {
            // Map from DTO to Domain Model
            CreateMap<Order, CreateOrderRequest>();
            CreateMap<CreateOrderResponseEntity, GetOrder>()
                .ForMember(dest => dest.OrderNo, opt => opt.MapFrom(src => src.OrderNo))
                .ForMember(dest => dest.ItemId, opt => opt.MapFrom(src => src.ItemId ?? ""))
                .ForMember(dest => dest.ItemCount, opt => opt.MapFrom(src => src.ItemCount ?? 0))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email ?? ""));
            CreateMap<GetOrder, OrderMessage>();
        }
    }
}
