using AutoMapper;
using VShop.ProductApi.Application.DTOs;
using VShop.ProductApi.Domain.Interfaces;

namespace VShop.ProductApi.Application.UseCases.Products;

public class GetAllProductsUseCase
{
    private readonly IProductRepository _repository;
    private readonly IMapper _mapper;

    public GetAllProductsUseCase(IProductRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ProductDTO>> ExecuteAsync()
    {
        var products = await _repository.GetAllAsync();
        return _mapper.Map<IEnumerable<ProductDTO>>(products);
    }
}
