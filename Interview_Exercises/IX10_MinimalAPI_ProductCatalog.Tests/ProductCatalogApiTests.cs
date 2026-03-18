using FluentAssertions;
using IX10_MinimalAPI_ProductCatalog.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace IX10_MinimalAPI_ProductCatalog.Tests;

public class ProductCatalogApiTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public ProductCatalogApiTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetProducts_ShouldReturnPagedResults()
    {
        var response = await _client.GetAsync("/products");
        response.EnsureSuccessStatusCode();

        var paged = await response.Content.ReadFromJsonAsync<PagedResponse<Product>>();

        paged.Should().NotBeNull();
        paged!.Items.Count.Should().BeGreaterThan(0);
        paged!.Page.Should().Be(1);
        paged!.PageSize.Should().Be(10);
    }

    [Fact]
    public async Task GetProducts_FilterByCategory_ShouldReturnFiltered()
    {
        var response = await _client.GetAsync("/products?category=Electronics");
        var paged = await response.Content.ReadFromJsonAsync<PagedResponse<Product>>();

        paged.Should().NotBeNull();
        paged!.Items.Should().AllSatisfy(p => p.Category.Should().Be("Electronics"));
    }

    [Fact]
    public async Task GetProducts_FilterByPriceRange_ShouldReturnFiltered()
    {
        var response = await _client.GetAsync("/products?minPrice=10&maxPrice=50");
        var paged = await response.Content.ReadFromJsonAsync<PagedResponse<Product>>();

        paged.Should().NotBeNull();
        paged!.Items.Should().NotBeEmpty();
        paged!.Items.Should().AllSatisfy(p =>
        {
            p.Price.Should().BeGreaterThanOrEqualTo(10);
            p.Price.Should().BeLessThanOrEqualTo(50);
        });
    }

    [Fact]
    public async Task GetProducts_SearchByName_ShouldReturnFiltered()
    {
        var response = await _client.GetAsync("/products?search=mouse");
        var paged = await response.Content.ReadFromJsonAsync<PagedResponse<Product>>();

        paged.Should().NotBeNull();
        paged!.Items.Should().NotBeEmpty();
        paged!.Items.Should().AllSatisfy(p =>
        {
            p.Name.Should().ContainEquivalentOf("mouse");
        });
    }

    [Fact]
    public async Task GetProducts_Pagination()
    {
        var response = await _client.GetAsync("/products?page=2&pageSize=2");
        var paged = await response.Content.ReadFromJsonAsync<PagedResponse<Product>>();

        paged.Should().NotBeNull();
        paged!.Page.Should().Be(2);
        paged!.PageSize.Should().Be(2);
        paged!.Items.Count.Should().BeLessThanOrEqualTo(2);
    }

    [Fact]
    public async Task GetProductById_Exists_ShouldReturnStatusCodeOk()
    {
        var request = new CreateProductRequest("Test Product", "Test Description", 9.99m, "TestCategory");
        var postResponse = await _client.PostAsJsonAsync("/products", request);
        var created = await postResponse.Content.ReadFromJsonAsync<Product>();

        var response = await _client.GetAsync($"/products/{created!.Id}");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetProductById_InexistentId_ShouldReturnStatusCode404()
    {
        var response = await _client.GetAsync($"/products/{Guid.NewGuid()}");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task PostProduct_Valid_ShouldReturnStatusCode201()
    {
        var request = new CreateProductRequest("Test Product", "Test Description", 9.99m, "TestCategory");
        var response = await _client.PostAsJsonAsync("/products", request);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        response.Headers.Location.Should().NotBeNull();

        var created = await response.Content.ReadFromJsonAsync<Product>();
        created.Should().NotBeNull();
        created.Name.Should().Be("Test Product");
    }

    [Fact]
    public async Task PostProduct_InvalidEmptyName_ShouldReturnStatusCode400()
    {
        var request = new CreateProductRequest("", "Test Description", 9.99m, "TestCategory");
        var response = await _client.PostAsJsonAsync("/products", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task PostProduct_InvalidNegativePrice_ShouldReturnStatusCode400()
    {
        var request = new CreateProductRequest("Test Product", "Test Description", -9.99m, "TestCategory");
        var response = await _client.PostAsJsonAsync("/products", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task PostProduct_InvalidEmptyCategory_ShouldReturnStatusCode400()
    {
        var request = new CreateProductRequest("Test Product", "Test Description", 9.99m, "");
        var response = await _client.PostAsJsonAsync("/products", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task PutProduct_Valid_ShouldReturnStatusCode200()
    {
        var request = new CreateProductRequest("Original", "", 9.99m, "OriginalCategory");
        var postResponse = await _client.PostAsJsonAsync("/products", request);
        var created = await postResponse.Content.ReadFromJsonAsync<Product>();

        var update = new UpdateProductRequest("Updated", "New Description", 19.99m, "NewCategory");
        var response = await _client.PutAsJsonAsync($"/products/{created!.Id}", update);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var updated = await response.Content.ReadFromJsonAsync<Product>();
        updated.Should().NotBeNull();
        updated!.Name.Should().Be("Updated");
        updated!.Price.Should().Be(19.99m);
        updated!.Category.Should().Be("NewCategory");
    }

    [Fact]
    public async Task PutProduct_InexistentId_ShouldReturnStatusCode404()
    {
        var update = new UpdateProductRequest("Updated", "New Description", 19.99m, "NewCategory");
        var response = await _client.PutAsJsonAsync($"/products/{Guid.NewGuid()}", update);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task PutProduct_InvalidEmptyName_ShouldReturnStatusCode400()
    {
        var request = new CreateProductRequest("Original", "", 9.99m, "OriginalCategory");
        var postResponse = await _client.PostAsJsonAsync("/products", request);
        var created = await postResponse.Content.ReadFromJsonAsync<Product>();

        var update = new UpdateProductRequest("", "Test Description", 9.99m, "TestCategory");
        var response = await _client.PutAsJsonAsync($"/products/{created!.Id}", update);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task PutProduct_InvalidNegativePrice_ShouldReturnStatusCode400()
    {
        var request = new CreateProductRequest("Original", "", 9.99m, "OriginalCategory");
        var postResponse = await _client.PostAsJsonAsync("/products", request);
        var created = await postResponse.Content.ReadFromJsonAsync<Product>();

        var update = new UpdateProductRequest("Updated", "Test Description", -9.99m, "TestCategory");
        var response = await _client.PutAsJsonAsync($"/products/{created!.Id}", update);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task PutProduct_InvalidEmptyCategory_ShouldReturnStatusCode400()
    {
        var request = new CreateProductRequest("Original", "", 9.99m, "OriginalCategory");
        var postResponse = await _client.PostAsJsonAsync("/products", request);
        var created = await postResponse.Content.ReadFromJsonAsync<Product>();

        var update = new UpdateProductRequest("Updated", "Test Description", 9.99m, "");
        var response = await _client.PutAsJsonAsync($"/products/{created!.Id}", update);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task DeleteProduct_Valid_ShouldReturnStatusCode204ThenSubsequentGetShouldReturn404()
    {
        var request = new CreateProductRequest("Original", "", 9.99m, "OriginalCategory");
        var postResponse = await _client.PostAsJsonAsync("/products", request);
        var created = await postResponse.Content.ReadFromJsonAsync<Product>();

        var deleteResponse = await _client.DeleteAsync($"/products/{created!.Id}");

        deleteResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var getResponse = await _client.GetAsync($"/products/{created!.Id}");

        getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task DeleteProduct_Invalid_ShouldReturn404()
    {
        var deleteResponse = await _client.DeleteAsync($"/products/{Guid.NewGuid()}");

        deleteResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }



}

