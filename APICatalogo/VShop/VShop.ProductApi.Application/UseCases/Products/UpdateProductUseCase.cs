using AutoMapper;
using VShop.ProductApi.Application.DTOs;
using VShop.ProductApi.Domain.Entities;
using VShop.ProductApi.Domain.Interfaces;

namespace VShop.ProductApi.Application.UseCases.Products;

public class UpdateProductUseCase
{
    private readonly IProductRepository _repository;
    private readonly IMapper _mapper;

    public UpdateProductUseCase(IProductRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ProductDTO> ExecuteAsync(ProductDTO dto)
    {
        var product = _mapper.Map<Product>(dto);
        product.Validate();
        await _repository.UpdateAsync(product);
        return _mapper.Map<ProductDTO>(product);
    }
}
