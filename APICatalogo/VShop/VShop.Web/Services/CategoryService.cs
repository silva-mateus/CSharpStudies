using System.Net.Http.Headers;
using System.Text.Json;
using VShop.Web.Models;
using VShop.Web.Services.Interfaces;

namespace VShop.Web.Services;

public class CategoryService : ICategoryService
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly ILogger<CategoryService> _logger;
    private const string apiEndpoint = "/api/categories";
    private readonly JsonSerializerOptions _options;

    public CategoryService(IHttpClientFactory clientFactory, ILogger<CategoryService> logger)
    {
        _clientFactory = clientFactory;
        _logger = logger;
        _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    }

    public async Task<IEnumerable<CategoryViewModel>?> GetCategoriesAsync(string token)
    {
        var client = _clientFactory.CreateClient("ProductApi");
        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);

        using var response = await client.GetAsync(apiEndpoint);

        if (response.IsSuccessStatusCode)
        {
            var apiResponse = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<IEnumerable<CategoryViewModel>>(apiResponse, _options);
        }

        _logger.LogError("Failed to get categories. Status: {StatusCode}", response.StatusCode);
        return null;
    }
}
