using VShop.CartApi.Domain.Interfaces;

namespace VShop.CartApi.Application.UseCases;

public class RemoveCouponUseCase
{
    private readonly ICartRepository _repository;

    public RemoveCouponUseCase(ICartRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> ExecuteAsync(string userId)
    {
        return await _repository.DeleteCouponAsync(userId);
    }
}
