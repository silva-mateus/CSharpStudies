using AutoMapper;
using VShop.ProductApi.Application.DTOs;
using VShop.ProductApi.Domain.Interfaces;

namespace VShop.ProductApi.Application.UseCases.Products;

public class GetProductByIdUseCase
{
    private readonly IProductRepository _repository;
    private readonly IMapper _mapper;

    public GetProductByIdUseCase(IProductRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ProductDTO?> ExecuteAsync(int id)
    {
        var product = await _repository.GetByIdAsync(id);
        return product is null ? null : _mapper.Map<ProductDTO>(product);
    }
}
