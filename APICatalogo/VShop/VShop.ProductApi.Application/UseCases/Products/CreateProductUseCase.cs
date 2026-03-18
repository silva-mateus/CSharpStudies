using AutoMapper;
using VShop.ProductApi.Application.DTOs;
using VShop.ProductApi.Domain.Entities;
using VShop.ProductApi.Domain.Interfaces;

namespace VShop.ProductApi.Application.UseCases.Products;

public class CreateProductUseCase
{
    private readonly IProductRepository _repository;
    private readonly IMapper _mapper;

    public CreateProductUseCase(IProductRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ProductDTO> ExecuteAsync(ProductDTO dto)
    {
        var product = _mapper.Map<Product>(dto);
        product.Validate();
        await _repository.CreateAsync(product);
        return _mapper.Map<ProductDTO>(product);
    }
}
