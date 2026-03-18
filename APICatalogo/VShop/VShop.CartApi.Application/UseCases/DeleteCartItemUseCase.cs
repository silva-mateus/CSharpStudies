using VShop.CartApi.Domain.Interfaces;

namespace VShop.CartApi.Application.UseCases;

public class DeleteCartItemUseCase
{
    private readonly ICartRepository _repository;

    public DeleteCartItemUseCase(ICartRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> ExecuteAsync(int cartItemId)
    {
        return await _repository.DeleteItemCartAsync(cartItemId);
    }
}
