using System.Net.Http.Headers;
using System.Text.Json;
using VShop.Web.Models;
using VShop.Web.Services.Interfaces;

namespace VShop.Web.Services;

public class CouponService : ICouponService
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly ILogger<CouponService> _logger;
    private const string apiEndpoint = "/api/coupon";
    private readonly JsonSerializerOptions _options;

    public CouponService(IHttpClientFactory clientFactory, ILogger<CouponService> logger)
    {
        _clientFactory = clientFactory;
        _logger = logger;
        _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    }

    public async Task<CouponViewModel?> GetCouponByCodeAsync(string couponCode, string token)
    {
        var client = _clientFactory.CreateClient("DiscountApi");
        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);

        using var response = await client.GetAsync($"{apiEndpoint}/{couponCode}");

        if (response.IsSuccessStatusCode)
        {
            var apiResponse = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<CouponViewModel>(apiResponse, _options);
        }

        _logger.LogError("Failed to get coupon {CouponCode}. Status: {StatusCode}", couponCode, response.StatusCode);
        return null;
    }
}
