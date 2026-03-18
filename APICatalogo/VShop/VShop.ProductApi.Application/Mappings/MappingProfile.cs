using AutoMapper;
using VShop.ProductApi.Application.DTOs;
using VShop.ProductApi.Domain.Entities;

namespace VShop.ProductApi.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Category, CategoryDTO>().ReverseMap();
        CreateMap<Product, ProductDTO>()
            .ForMember(dest => dest.CategoryName,
                       opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : string.Empty))
            .ReverseMap()
            .ForMember(dest => dest.Category, opt => opt.Ignore());
    }
}
