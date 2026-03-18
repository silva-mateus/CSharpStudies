using VShop.ProductApi.Domain.Interfaces;

namespace VShop.ProductApi.Application.UseCases.Categories;

public class DeleteCategoryUseCase
{
    private readonly ICategoryRepository _repository;

    public DeleteCategoryUseCase(ICategoryRepository repository)
    {
        _repository = repository;
    }

    public async Task ExecuteAsync(int id)
    {
        await _repository.DeleteAsync(id);
    }
}
