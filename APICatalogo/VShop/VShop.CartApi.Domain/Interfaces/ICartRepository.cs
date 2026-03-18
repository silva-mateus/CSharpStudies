using VShop.CartApi.Domain.Entities;

namespace VShop.CartApi.Domain.Interfaces;

public interface ICartRepository
{
    Task<Cart> GetCartByUserIdAsync(string userId);
    Task<Cart> UpdateCartAsync(Cart cart);
    Task<bool> DeleteItemCartAsync(int cartItemId);
    Task<bool> ApplyCouponAsync(string userId, string couponCode);
    Task<bool> DeleteCouponAsync(string userId);
    Task<bool> CleanCartAsync(string userId);
}
