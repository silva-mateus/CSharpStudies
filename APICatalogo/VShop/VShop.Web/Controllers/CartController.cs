using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VShop.Web.Models;
using VShop.Web.Services.Interfaces;

namespace VShop.Web.Controllers;

[Authorize]
public class CartController : Controller
{
    private readonly ICartService _cartService;
    private readonly ICouponService _couponService;

    public CartController(ICartService cartService, ICouponService couponService)
    {
        _cartService = cartService;
        _couponService = couponService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var cart = await GetCartByUser();

        if (cart is null)
            return View(new CartViewModel());

        await ApplyDiscountToCart(cart);

        return View(cart);
    }

    [HttpPost]
    public async Task<IActionResult> ApplyCoupon(CartViewModel cart)
    {
        var token = await GetAccessTokenAsync();
        var result = await _cartService.ApplyCouponAsync(cart, token);

        if (result)
            return RedirectToAction(nameof(Index));

        return View("Index", cart);
    }

    [HttpPost]
    public async Task<IActionResult> RemoveCoupon()
    {
        var token = await GetAccessTokenAsync();
        var userId = GetUserId();

        var result = await _cartService.RemoveCouponAsync(userId, token);

        if (result)
            return RedirectToAction(nameof(Index));

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> RemoveItem(int id)
    {
        var token = await GetAccessTokenAsync();
        var cart = await GetCartByUser();

        var item = cart?.CartItems.FirstOrDefault(c => c.Id == id);

        if (item is null)
            return View("Error");

        return View(item);
    }

    [HttpPost]
    [ActionName("RemoveItem")]
    public async Task<IActionResult> RemoveItemConfirmed(int id)
    {
        var token = await GetAccessTokenAsync();
        var result = await _cartService.RemoveItemFromCartAsync(id, token);

        if (result)
            return RedirectToAction(nameof(Index));

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Checkout()
    {
        var cart = await GetCartByUser();

        if (cart?.CartItems is null || !cart.CartItems.Any())
            return RedirectToAction(nameof(Index));

        await ApplyDiscountToCart(cart);
        cart.CartHeader.CartTotalItems = cart.CartItems.Count();

        return View(cart);
    }

    [HttpPost]
    [ActionName("Checkout")]
    public async Task<IActionResult> CheckoutPost(CartViewModel cart)
    {
        if (!ModelState.IsValid)
        {
            var fullCart = await GetCartByUser();
            if (fullCart is null)
                return RedirectToAction(nameof(Index));

            fullCart.CartHeader.FirstName = cart.CartHeader.FirstName;
            fullCart.CartHeader.LastName = cart.CartHeader.LastName;
            fullCart.CartHeader.Phone = cart.CartHeader.Phone;
            fullCart.CartHeader.Email = cart.CartHeader.Email;
            fullCart.CartHeader.CardNumber = cart.CartHeader.CardNumber;
            fullCart.CartHeader.NameOnCard = cart.CartHeader.NameOnCard;
            fullCart.CartHeader.CVV = cart.CartHeader.CVV;
            fullCart.CartHeader.ExpiryMonthYear = cart.CartHeader.ExpiryMonthYear;
            fullCart.CartHeader.CouponCode = cart.CartHeader.CouponCode;
            fullCart.CartHeader.Discount = cart.CartHeader.Discount;
            fullCart.CartHeader.TotalAmount = cart.CartHeader.TotalAmount;
            fullCart.CartHeader.CartTotalItems = fullCart.CartItems.Count();

            await ApplyDiscountToCart(fullCart);
            return View(fullCart);
        }

        var token = await GetAccessTokenAsync();
        var result = await _cartService.CheckoutAsync(cart.CartHeader, token);

        if (result is not null)
            return RedirectToAction(nameof(CheckoutCompleted));

        return View(cart);
    }

    [HttpGet]
    public IActionResult CheckoutCompleted()
    {
        return View();
    }

    private async Task<CartViewModel?> GetCartByUser()
    {
        var token = await GetAccessTokenAsync();
        var userId = GetUserId();
        return await _cartService.GetCartByUserIdAsync(userId, token);
    }

    private string GetUserId()
    {
        return User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
    }

    private async Task<string> GetAccessTokenAsync()
    {
        return await HttpContext.GetTokenAsync("access_token") ?? string.Empty;
    }

    private async Task ApplyDiscountToCart(CartViewModel cart)
    {
        if (string.IsNullOrEmpty(cart.CartHeader.CouponCode))
            return;

        var token = await GetAccessTokenAsync();
        var coupon = await _couponService.GetCouponByCodeAsync(cart.CartHeader.CouponCode, token);

        if (coupon is not null)
            cart.CartHeader.Discount = coupon.Discount;
    }
}
