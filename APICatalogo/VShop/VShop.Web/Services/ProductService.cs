using System.Net.Http.Headers;
using System.Text.Json;
using VShop.Web.Models;
using VShop.Web.Services.Interfaces;

namespace VShop.Web.Services;

public class ProductService : IProductService
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly ILogger<ProductService> _logger;
    private const string apiEndpoint = "/api/products";
    private readonly JsonSerializerOptions _options;

    public ProductService(IHttpClientFactory clientFactory, ILogger<ProductService> logger)
    {
        _clientFactory = clientFactory;
        _logger = logger;
        _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    }

    public async Task<IEnumerable<ProductViewModel>?> GetProductsAsync(string token)
    {
        var client = CreateClient(token);

        using var response = await client.GetAsync(apiEndpoint);

        if (response.IsSuccessStatusCode)
        {
            var apiResponse = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<IEnumerable<ProductViewModel>>(apiResponse, _options);
        }

        _logger.LogError("Failed to get products. Status: {StatusCode}", response.StatusCode);
        return null;
    }

    public async Task<ProductViewModel?> GetProductByIdAsync(int id, string token)
    {
        var client = CreateClient(token);

        using var response = await client.GetAsync($"{apiEndpoint}/{id}");

        if (response.IsSuccessStatusCode)
        {
            var apiResponse = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<ProductViewModel>(apiResponse, _options);
        }

        _logger.LogError("Failed to get product {Id}. Status: {StatusCode}", id, response.StatusCode);
        return null;
    }

    public async Task<ProductViewModel?> CreateProductAsync(ProductViewModel productViewModel, string token)
    {
        var client = CreateClient(token);

        var content = new StringContent(
            JsonSerializer.Serialize(productViewModel),
            System.Text.Encoding.UTF8,
            "application/json");

        using var response = await client.PostAsync(apiEndpoint, content);

        if (response.IsSuccessStatusCode)
        {
            var apiResponse = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<ProductViewModel>(apiResponse, _options);
        }

        _logger.LogError("Failed to create product. Status: {StatusCode}", response.StatusCode);
        return null;
    }

    public async Task<ProductViewModel?> UpdateProductAsync(ProductViewModel product, string token)
    {
        var client = CreateClient(token);

        using var response = await client.PutAsJsonAsync(apiEndpoint, product);

        if (response.IsSuccessStatusCode)
        {
            var apiResponse = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<ProductViewModel>(apiResponse, _options);
        }

        _logger.LogError("Failed to update product {Id}. Status: {StatusCode}", product.Id, response.StatusCode);
        return null;
    }

    public async Task<bool> DeleteProductAsync(int id, string token)
    {
        var client = CreateClient(token);

        using var response = await client.DeleteAsync($"{apiEndpoint}/{id}");

        if (!response.IsSuccessStatusCode)
            _logger.LogError("Failed to delete product {Id}. Status: {StatusCode}", id, response.StatusCode);

        return response.IsSuccessStatusCode;
    }

    private HttpClient CreateClient(string token)
    {
        var client = _clientFactory.CreateClient("ProductApi");
        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);
        return client;
    }
}
