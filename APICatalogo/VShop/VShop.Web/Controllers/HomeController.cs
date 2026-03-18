using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VShop.Web.Models;
using VShop.Web.Services.Interfaces;

namespace VShop.Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IProductService _productService;
    private readonly ICartService _cartService;

    public HomeController(ILogger<HomeController> logger,
                          IProductService productService,
                          ICartService cartService)
    {
        _logger = logger;
        _productService = productService;
        _cartService = cartService;
    }

    [Authorize]
    public async Task<IActionResult> Index()
    {
        var token = await GetAccessTokenAsync();
        var products = await _productService.GetProductsAsync(token);

        if (products is null)
            return View("Error");

        return View(products);
    }

    [Authorize]
    public async Task<IActionResult> ProductDetails(int id)
    {
        var token = await GetAccessTokenAsync();
        var product = await _productService.GetProductByIdAsync(id, token);

        if (product is null)
            return View("Error");

        return View(product);
    }

    [Authorize]
    [HttpPost]
    [ActionName("ProductDetailsPost")]
    public async Task<IActionResult> ProductDetailsPost(ProductViewModel productViewModel, int count)
    {
        var token = await GetAccessTokenAsync();
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;

        var cart = new CartViewModel
        {
            CartHeader = new CartHeaderViewModel
            {
                UserId = userId
            },
            CartItems = new List<CartItemViewModel>
            {
                new CartItemViewModel
                {
                    ProductId = productViewModel.Id,
                    Quantity = count,
                    Product = productViewModel
                }
            }
        };

        var result = await _cartService.AddItemToCartAsync(cart, token);

        if (result is not null)
            return RedirectToAction("Index", "Cart");

        return View("ProductDetails", productViewModel);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    private async Task<string> GetAccessTokenAsync()
    {
        return await HttpContext.GetTokenAsync("access_token") ?? string.Empty;
    }
}
