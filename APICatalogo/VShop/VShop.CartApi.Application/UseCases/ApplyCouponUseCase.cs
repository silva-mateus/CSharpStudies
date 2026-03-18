using VShop.CartApi.Domain.Interfaces;

namespace VShop.CartApi.Application.UseCases;

public class ApplyCouponUseCase
{
    private readonly ICartRepository _repository;

    public ApplyCouponUseCase(ICartRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> ExecuteAsync(string userId, string couponCode)
    {
        return await _repository.ApplyCouponAsync(userId, couponCode);
    }
}
