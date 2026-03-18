using VShop.Web.Models;

namespace VShop.Web.Services.Interfaces;

public interface ICartService
{
    Task<CartViewModel?> GetCartByUserIdAsync(string userId, string token);
    Task<CartViewModel?> AddItemToCartAsync(CartViewModel cart, string token);
    Task<CartViewModel?> UpdateCartAsync(CartViewModel cart, string token);
    Task<bool> RemoveItemFromCartAsync(int cartItemId, string token);
    Task<bool> ApplyCouponAsync(CartViewModel cart, string token);
    Task<bool> RemoveCouponAsync(string userId, string token);
    Task<bool> ClearCartAsync(string userId, string token);
    Task<CartHeaderViewModel?> CheckoutAsync(CartHeaderViewModel cartHeader, string token);
}
