using AutoMapper;
using VShop.ProductApi.Application.DTOs;
using VShop.ProductApi.Domain.Interfaces;

namespace VShop.ProductApi.Application.UseCases.Categories;

public class GetCategoriesWithProductsUseCase
{
    private readonly ICategoryRepository _repository;
    private readonly IMapper _mapper;

    public GetCategoriesWithProductsUseCase(ICategoryRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CategoryDTO>> ExecuteAsync()
    {
        var categories = await _repository.GetCategoriesWithProductsAsync();
        return _mapper.Map<IEnumerable<CategoryDTO>>(categories);
    }
}
