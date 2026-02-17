using System.Collections.Concurrent;
using System.Globalization;

namespace JD05_Debugging_Challenge;

/// <summary>
/// This file contains 10 deliberate bugs. Find and fix them all.
/// DO NOT change method signatures -- only fix the internal logic.
/// </summary>
public class ProductService
{
    private readonly List<Product> _products;

    public ProductService(List<Product> products)
    {
        _products = products;
    }

    // BUG #1: Off-by-one error in pagination.
    // Page 1 should return items 0..(pageSize-1), but this skips the first item.
    public PagedResult<Product> GetPaged(int page, int pageSize)
    {
        var skip = (page - 1) * pageSize; // BUG: should be (page - 1) * pageSize for 1-based pages
        var items = _products.Skip(skip).Take(pageSize).ToList();

        return new PagedResult<Product>
        {
            Items = items,
            TotalCount = _products.Count,
            Page = page,
            PageSize = pageSize
        };
    }

    // BUG #8: Wrong LINQ predicate -- compares Name instead of Id.
    public Product? GetById(int id)
    {
        return _products.FirstOrDefault(p => p.Name.Length == id); // BUG: should be p.Id == id
    }

    // BUG #9: Culture-sensitive comparison fails for Turkish locale.
    // "WIFI" vs "wifi" comparison fails when culture is Turkish (I vs İ).
    public List<Product> SearchByCategory(string category)
    {
        return _products
            .Where(p => p.Category.ToUpper() == category.ToUpper()) // BUG: should use OrdinalIgnoreCase
            .ToList();
    }
}

public class OrderCache
{
    // BUG #2: Race condition -- Dictionary is not thread-safe.
    private readonly ConcurrentDictionary<int, Order> _cache = new(); // BUG: should be ConcurrentDictionary

    public void AddOrder(Order order)
    {
        _cache[order.Id] = order;
    }

    public Order? GetOrder(int id)
    {
        return _cache.TryGetValue(id, out var order) ? order : null;
    }

    public int Count => _cache.Count;
}

public class QueryBuilder
{
    // BUG #3: SQL injection vulnerability -- string concatenation.
    public (string Query, string ParamName, string ParamValue) BuildProductQuery(string categoryFilter)
    {
        // BUG: directly concatenating user input into SQL. Should use parameterized query.
        return ("SELECT * FROM Products WHERE Category = @Category", "@Category", categoryFilter);
    }

    /// <summary>
    /// Returns a safe parameterized query string and the parameter value.
    /// This is the correct pattern -- but the method above is the buggy one.
    /// </summary>
    public (string Query, string ParamName, string ParamValue) BuildSafeProductQuery(string categoryFilter)
    {
        return ("SELECT * FROM Products WHERE Category = @Category", "@Category", categoryFilter);
    }
}

public class AsyncService
{
    // BUG #4: Async deadlock -- .Result blocks the thread.
    public string GetDataSync()
    {
        return GetDataAsync().Result; // BUG: causes deadlock in sync contexts. Should use async all the way.
    }

    public async Task<string> GetDataAsync()
    {
        await Task.Delay(10);
        return "async data";
    }
}

public class OrderProcessor
{
    // BUG #5: Null reference -- DiscountPercent can be null.
    public decimal CalculateTotal(Order order)
    {
        var subtotal = order.Lines.Sum(l => l.Quantity * l.UnitPrice);
        var discount = subtotal * (order.DiscountPercent.Value / 100m); // BUG: .Value on nullable without check
        return subtotal - discount;
    }

    // BUG #6: Wrong rounding mode -- Banker's rounding vs standard.
    public decimal RoundTotal(decimal total)
    {
        return Math.Round(total, 2); // BUG: default is MidpointRounding.ToEven (Banker's). Should be AwayFromZero.
    }
}

public class ExternalApiClient
{
    // BUG #7: Resource leak -- creating HttpClient in a loop without disposing.
    public async Task<List<string>> FetchMultipleEndpoints(string[] urls)
    {
        var results = new List<string>();
        foreach (var url in urls)
        {
            var client = new HttpClient(); // BUG: should be shared or disposed. Creates socket leak.
            try
            {
                var result = await client.GetStringAsync(url);
                results.Add(result);
            }
            catch
            {
                results.Add("error");
            }
        }
        return results;
    }
}

public class NotificationService
{
    private readonly List<string> _sentNotifications = new();

    // BUG #10: Exception swallowed silently -- empty catch hides errors.
    public bool SendNotification(string message)
    {
        try
        {
            if (string.IsNullOrEmpty(message))
                throw new ArgumentException("Message cannot be empty.", nameof(message));

            _sentNotifications.Add(message);
            return true;
        }
        catch
        {
            // BUG: silently swallows the exception. Should rethrow or return false with logging.
            return true; // BUG: returns true even when it failed!
        }
    }

    public IReadOnlyList<string> GetSentNotifications() => _sentNotifications.AsReadOnly();
}
