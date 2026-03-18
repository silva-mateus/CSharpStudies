using VShop.ProductApi.Domain.Interfaces;

namespace VShop.ProductApi.Application.UseCases.Products;

public class DeleteProductUseCase
{
    private readonly IProductRepository _repository;

    public DeleteProductUseCase(IProductRepository repository)
    {
        _repository = repository;
    }

    public async Task ExecuteAsync(int id)
    {
        await _repository.DeleteAsync(id);
    }
}
