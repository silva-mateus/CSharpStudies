using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using VShop.Web.Models;
using VShop.Web.Services.Interfaces;

namespace VShop.Web.Services;

public class CartService : ICartService
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly ILogger<CartService> _logger;
    private readonly JsonSerializerOptions _options;
    private const string apiEndpoint = "/api/cart";

    public CartService(IHttpClientFactory clientFactory, ILogger<CartService> logger)
    {
        _clientFactory = clientFactory;
        _logger = logger;
        _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    }

    public async Task<CartViewModel?> GetCartByUserIdAsync(string userId, string token)
    {
        var client = CreateClient(token);

        using var response = await client.GetAsync($"{apiEndpoint}/getcart/{userId}");

        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<CartViewModel>(content, _options);
        }

        _logger.LogError("Failed to get cart for user {UserId}. Status: {StatusCode}", userId, response.StatusCode);
        return null;
    }

    public async Task<CartViewModel?> AddItemToCartAsync(CartViewModel cart, string token)
    {
        var client = CreateClient(token);

        var content = new StringContent(
            JsonSerializer.Serialize(cart), Encoding.UTF8, "application/json");

        using var response = await client.PostAsync($"{apiEndpoint}/addcart", content);

        if (response.IsSuccessStatusCode)
        {
            var apiResponse = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<CartViewModel>(apiResponse, _options);
        }

        _logger.LogError("Failed to add item to cart. Status: {StatusCode}", response.StatusCode);
        return null;
    }

    public async Task<CartViewModel?> UpdateCartAsync(CartViewModel cart, string token)
    {
        var client = CreateClient(token);

        using var response = await client.PutAsJsonAsync($"{apiEndpoint}/updatecart", cart);

        if (response.IsSuccessStatusCode)
        {
            var apiResponse = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<CartViewModel>(apiResponse, _options);
        }

        _logger.LogError("Failed to update cart. Status: {StatusCode}", response.StatusCode);
        return null;
    }

    public async Task<bool> RemoveItemFromCartAsync(int cartItemId, string token)
    {
        var client = CreateClient(token);

        using var response = await client.DeleteAsync($"{apiEndpoint}/deletecart/{cartItemId}");

        if (!response.IsSuccessStatusCode)
            _logger.LogError("Failed to remove cart item {CartItemId}. Status: {StatusCode}", cartItemId, response.StatusCode);

        return response.IsSuccessStatusCode;
    }

    public async Task<bool> ApplyCouponAsync(CartViewModel cart, string token)
    {
        var client = CreateClient(token);

        var content = new StringContent(
            JsonSerializer.Serialize(cart), Encoding.UTF8, "application/json");

        using var response = await client.PutAsync($"{apiEndpoint}/applycoupon", content);

        if (!response.IsSuccessStatusCode)
            _logger.LogError("Failed to apply coupon. Status: {StatusCode}", response.StatusCode);

        return response.IsSuccessStatusCode;
    }

    public async Task<bool> RemoveCouponAsync(string userId, string token)
    {
        var client = CreateClient(token);

        using var response = await client.DeleteAsync($"{apiEndpoint}/deletecoupon/{userId}");

        if (!response.IsSuccessStatusCode)
            _logger.LogError("Failed to remove coupon for user {UserId}. Status: {StatusCode}", userId, response.StatusCode);

        return response.IsSuccessStatusCode;
    }

    public async Task<bool> ClearCartAsync(string userId, string token)
    {
        var client = CreateClient(token);

        var content = new StringContent(
            JsonSerializer.Serialize(new CartHeaderViewModel { UserId = userId }),
            Encoding.UTF8, "application/json");

        using var request = new HttpRequestMessage(HttpMethod.Delete, $"{apiEndpoint}/checkout")
        {
            Content = content
        };

        using var response = await client.SendAsync(request);

        if (!response.IsSuccessStatusCode)
            _logger.LogError("Failed to clear cart for user {UserId}. Status: {StatusCode}", userId, response.StatusCode);

        return response.IsSuccessStatusCode;
    }

    public async Task<CartHeaderViewModel?> CheckoutAsync(
        CartHeaderViewModel cartHeader, string token)
    {
        var client = CreateClient(token);

        var content = new StringContent(
            JsonSerializer.Serialize(cartHeader), Encoding.UTF8, "application/json");

        using var response = await client.PostAsync($"{apiEndpoint}/checkout", content);

        if (response.IsSuccessStatusCode)
        {
            var apiResponse = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<CartHeaderViewModel>(apiResponse, _options);
        }

        _logger.LogError("Failed to checkout. Status: {StatusCode}", response.StatusCode);
        return null;
    }

    private HttpClient CreateClient(string token)
    {
        var client = _clientFactory.CreateClient("CartApi");
        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);
        return client;
    }
}
