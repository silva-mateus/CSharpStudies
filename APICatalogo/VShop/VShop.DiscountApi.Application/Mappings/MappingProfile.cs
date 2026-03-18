using AutoMapper;
using VShop.DiscountApi.Application.DTOs;
using VShop.DiscountApi.Domain.Entities;

namespace VShop.DiscountApi.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Coupon, CouponDTO>().ReverseMap();
    }
}
