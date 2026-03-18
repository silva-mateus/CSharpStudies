using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using VShop.Web.Models;
using VShop.Web.Roles;
using VShop.Web.Services.Interfaces;

namespace VShop.Web.Controllers;

[Authorize(Roles = AppRoles.Admin)]
public class ProductsController : Controller
{
    private readonly IProductService _productService;
    private readonly ICategoryService _categoryService;

    public ProductsController(IProductService productService, ICategoryService categoryService)
    {
        _productService = productService;
        _categoryService = categoryService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductViewModel>>> Index()
    {
        var token = await GetAccessTokenAsync();
        var result = await _productService.GetProductsAsync(token);

        if (result == null)
            return View("Error");

        return View(result);
    }

    [HttpGet]
    public async Task<IActionResult> CreateProduct()
    {
        var token = await GetAccessTokenAsync();

        ViewBag.CategoryId = new SelectList(
            await _categoryService.GetCategoriesAsync(token), "Id", "Name");

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CreateProduct(ProductViewModel productViewModel)
    {
        if (ModelState.IsValid)
        {
            var token = await GetAccessTokenAsync();
            var result = await _productService.CreateProductAsync(productViewModel, token);

            if (result != null)
                return RedirectToAction(nameof(Index));
        }

        var reloadToken = await GetAccessTokenAsync();
        ViewBag.CategoryId = new SelectList(
            await _categoryService.GetCategoriesAsync(reloadToken), "Id", "Name");

        return View(productViewModel);
    }

    [HttpGet]
    public async Task<IActionResult> UpdateProduct(int id)
    {
        var token = await GetAccessTokenAsync();
        var result = await _productService.GetProductByIdAsync(id, token);

        if (result == null)
            return View("Error");

        ViewBag.CategoryId = new SelectList(
            await _categoryService.GetCategoriesAsync(token), "Id", "Name");

        return View(result);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateProduct(ProductViewModel productViewModel)
    {
        if (ModelState.IsValid)
        {
            var token = await GetAccessTokenAsync();
            var result = await _productService.UpdateProductAsync(productViewModel, token);

            if (result != null)
                return RedirectToAction(nameof(Index));
        }

        var reloadToken = await GetAccessTokenAsync();
        ViewBag.CategoryId = new SelectList(
            await _categoryService.GetCategoriesAsync(reloadToken), "Id", "Name");

        return View(productViewModel);
    }

    [HttpGet]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var token = await GetAccessTokenAsync();
        var result = await _productService.GetProductByIdAsync(id, token);

        if (result == null)
            return View("Error");

        return View(result);
    }

    [HttpPost]
    [ActionName("DeleteProduct")]
    public async Task<IActionResult> DeleteProductConfirmed(int id)
    {
        var token = await GetAccessTokenAsync();
        var result = await _productService.DeleteProductAsync(id, token);

        if (!result)
            return View("Error");

        return RedirectToAction(nameof(Index));
    }

    private async Task<string> GetAccessTokenAsync()
    {
        return await HttpContext.GetTokenAsync("access_token") ?? string.Empty;
    }
}
