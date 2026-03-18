using AutoMapper;
using VShop.CartApi.Application.DTOs;
using VShop.CartApi.Domain.Entities;

namespace VShop.CartApi.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CartHeader, CartHeaderDTO>().ReverseMap();
        CreateMap<CartItem, CartItemDTO>().ReverseMap();
        CreateMap<Cart, CartDTO>().ReverseMap();
        CreateMap<Product, ProductDTO>().ReverseMap();
    }
}
