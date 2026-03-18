using AutoMapper;
using VShop.ProductApi.Application.DTOs;
using VShop.ProductApi.Domain.Interfaces;

namespace VShop.ProductApi.Application.UseCases.Categories;

public class GetCategoryByIdUseCase
{
    private readonly ICategoryRepository _repository;
    private readonly IMapper _mapper;

    public GetCategoryByIdUseCase(ICategoryRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<CategoryDTO?> ExecuteAsync(int id)
    {
        var category = await _repository.GetByIdAsync(id);
        return category is null ? null : _mapper.Map<CategoryDTO>(category);
    }
}
