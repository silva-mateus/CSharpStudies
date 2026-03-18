using AutoMapper;
using VShop.ProductApi.Application.DTOs;
using VShop.ProductApi.Domain.Entities;
using VShop.ProductApi.Domain.Interfaces;

namespace VShop.ProductApi.Application.UseCases.Categories;

public class CreateCategoryUseCase
{
    private readonly ICategoryRepository _repository;
    private readonly IMapper _mapper;

    public CreateCategoryUseCase(ICategoryRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<CategoryDTO> ExecuteAsync(CategoryDTO dto)
    {
        var category = _mapper.Map<Category>(dto);
        category.Validate();
        await _repository.CreateAsync(category);
        return _mapper.Map<CategoryDTO>(category);
    }
}
