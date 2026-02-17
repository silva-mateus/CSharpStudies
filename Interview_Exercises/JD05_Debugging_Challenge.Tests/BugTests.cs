using FluentAssertions;
using JD05_Debugging_Challenge;
using Xunit;

namespace JD05_Debugging_Challenge.Tests;

/// <summary>
/// DO NOT MODIFY THESE TESTS. Fix the bugs in the source code to make them pass.
/// Each test targets one specific bug.
/// </summary>
public class BugTests
{
    private static List<Product> CreateTestProducts() => new()
    {
        new Product { Id = 1, Name = "Mouse", Price = 29.99m, Category = "Electronics" },
        new Product { Id = 2, Name = "Keyboard", Price = 89.99m, Category = "Electronics" },
        new Product { Id = 3, Name = "Monitor", Price = 299.99m, Category = "Electronics" },
        new Product { Id = 4, Name = "Desk", Price = 499.99m, Category = "Furniture" },
        new Product { Id = 5, Name = "Chair", Price = 349.99m, Category = "Furniture" },
        new Product { Id = 6, Name = "Notebook", Price = 12.99m, Category = "Stationery" },
        new Product { Id = 7, Name = "Pen", Price = 4.99m, Category = "Stationery" },
        new Product { Id = 8, Name = "Lamp", Price = 39.99m, Category = "Furniture" },
        new Product { Id = 9, Name = "Cable", Price = 9.99m, Category = "Electronics" },
        new Product { Id = 10, Name = "Book", Price = 19.99m, Category = "Books" },
    };

    // BUG #1: Off-by-one - Page 1 should return the first items
    [Fact]
    public void Bug01_Pagination_Page1_ShouldReturnFirstItems()
    {
        var service = new ProductService(CreateTestProducts());

        var result = service.GetPaged(page: 1, pageSize: 3);

        result.Items.Should().HaveCount(3);
        result.Items[0].Name.Should().Be("Mouse"); // First product, not second
    }

    // BUG #2: Race condition - concurrent access should not throw
    [Fact]
    public void Bug02_OrderCache_ConcurrentAccess_ShouldNotThrow()
    {
        var cache = new OrderCache();
        var tasks = new List<Task>();

        // Hammer the cache from multiple threads
        for (int i = 0; i < 1000; i++)
        {
            var id = i;
            tasks.Add(Task.Run(() => cache.AddOrder(new Order { Id = id, CustomerName = $"Customer {id}" })));
        }

        var act = () => Task.WaitAll(tasks.ToArray());
        act.Should().NotThrow();
        cache.Count.Should().Be(1000);
    }

    // BUG #3: SQL injection - query should use parameterized approach
    [Fact]
    public void Bug03_QueryBuilder_ShouldNotBeSqlInjectable()
    {
        var builder = new QueryBuilder();

        // This malicious input should NOT end up directly in the SQL string
        var maliciousInput = "'; DROP TABLE Products; --";
        var query = builder.BuildProductQuery(maliciousInput);

        // After fixing, BuildProductQuery should return a parameterized query
        // that does NOT contain the raw user input in the SQL text.
        query.Query.Should().NotContain(maliciousInput);
        query.Should().Contain("@");
    }

    // BUG #4: Async deadlock - sync wrapper should return data
    [Fact]
    public async Task Bug04_AsyncService_SyncWrapper_ShouldReturnData()
    {
        var service = new AsyncService();

        // After fix, this should use async properly instead of .Result
        var result = await service.GetDataAsync();

        result.Should().Be("async data");
    }

    // BUG #5: Null reference - should handle null discount
    [Fact]
    public void Bug05_OrderProcessor_NullDiscount_ShouldNotThrow()
    {
        var processor = new OrderProcessor();
        var order = new Order
        {
            Id = 1,
            CustomerName = "Test",
            DiscountPercent = null, // No discount
            Lines = new List<OrderLine>
            {
                new OrderLine { ProductId = 1, ProductName = "Item", Quantity = 2, UnitPrice = 50m }
            }
        };

        var total = processor.CalculateTotal(order);

        total.Should().Be(100m); // 2 * 50 = 100, no discount
    }

    // BUG #6: Rounding - 2.545 should round to 2.55 (standard), not 2.54 (banker's)
    [Fact]
    public void Bug06_OrderProcessor_Rounding_ShouldUseStandardRounding()
    {
        var processor = new OrderProcessor();

        var result = processor.RoundTotal(2.545m);

        result.Should().Be(2.55m); // Standard rounding, not Banker's (2.54)
    }

    // BUG #7: Resource leak - should use shared HttpClient
    [Fact]
    public async Task Bug07_ExternalApiClient_ShouldNotLeakResources()
    {
        var client = new ExternalApiClient();
        // We can't easily test socket leaks, but we can verify the method
        // doesn't throw and uses a single HttpClient instance.
        // After fix, this should use IHttpClientFactory or a shared instance.
        // For now, just verify it handles errors gracefully for invalid URLs.
        var urls = new[] { "http://localhost:1/nonexistent", "http://localhost:1/also-nonexistent" };

        var results = await client.FetchMultipleEndpoints(urls);

        results.Should().HaveCount(2);
        results.Should().AllBe("error");
    }

    // BUG #8: Wrong predicate - GetById should find by Id, not Name.Length
    [Fact]
    public void Bug08_ProductService_GetById_ShouldFindCorrectProduct()
    {
        var service = new ProductService(CreateTestProducts());

        var product = service.GetById(4);

        product.Should().NotBeNull();
        product!.Id.Should().Be(4);
        product.Name.Should().Be("Desk");
    }

    // BUG #9: Culture issue - category search should be culture-invariant
    [Fact]
    public void Bug09_ProductService_CategorySearch_ShouldBeCultureInvariant()
    {
        var service = new ProductService(CreateTestProducts());

        // This should work regardless of culture (Turkish-I problem)
        var results = service.SearchByCategory("electronics");

        results.Should().HaveCount(4);
        results.Should().OnlyContain(p => p.Category == "Electronics");
    }

    // BUG #10: Swallowed exception - should return false for invalid input
    [Fact]
    public void Bug10_NotificationService_EmptyMessage_ShouldReturnFalse()
    {
        var service = new NotificationService();

        var result = service.SendNotification("");

        result.Should().BeFalse(); // Empty message should fail, not succeed silently
        service.GetSentNotifications().Should().BeEmpty();
    }
}
