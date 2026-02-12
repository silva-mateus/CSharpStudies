using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using IX10_MinimalAPI_ProductCatalog.Models;
using Xunit;

namespace IX10_MinimalAPI_ProductCatalog.Tests;

/// <summary>
/// TODO: Write integration tests using WebApplicationFactory.
///
/// Setup:
///   - Create a WebApplicationFactory&lt;Program&gt; (the partial class in Program.cs).
///   - Use factory.CreateClient() to get an HttpClient.
///   - Seeded data includes products in categories: Electronics, Furniture, Books.
///
/// TESTS TO WRITE:
///
/// GET /products:
///  1. Returns 200 with paged results (default page=1, pageSize=10).
///  2. Filtering by category=Electronics returns only electronics.
///  3. Filtering by minPrice=10, maxPrice=50 returns products in range.
///  4. Search by name (search=mouse) returns matching products (case-insensitive).
///  5. Pagination: page=2, pageSize=2 returns correct page.
///
/// GET /products/{id}:
///  6. Returns 200 with product for valid ID (create one first via POST, then GET).
///  7. Returns 404 for non-existent ID.
///
/// POST /products:
///  8. Valid request returns 201 with product and Location header.
///  9. Invalid request (empty name) returns 400 with validation errors.
/// 10. Invalid request (negative price) returns 400.
///
/// PUT /products/{id}:
/// 11. Valid update returns 200 with updated product.
/// 12. Update non-existent ID returns 404.
///
/// DELETE /products/{id}:
/// 13. Delete existing product returns 204; subsequent GET returns 404.
/// 14. Delete non-existent ID returns 404.
///
/// Tips:
///   - Use response.Content.ReadFromJsonAsync&lt;T&gt;() to deserialize responses.
///   - Use HttpClient.PostAsJsonAsync, PutAsJsonAsync for sending JSON.
///   - For PagedResponse, create a local record type or use JsonDocument.
/// </summary>
public class ProductCatalogApiTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public ProductCatalogApiTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    // TODO: Write your integration tests here
}
