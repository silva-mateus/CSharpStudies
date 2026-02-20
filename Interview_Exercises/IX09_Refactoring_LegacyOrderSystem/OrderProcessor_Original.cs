namespace IX09_Refactoring_LegacyOrderSystem;

/// <summary>
/// LEGACY CODE - DO NOT MODIFY UNTIL YOU HAVE WRITTEN CHARACTERIZATION TESTS.
/// After writing tests, rename this file to OrderProcessor_Original.cs and keep it for reference.
///
/// This class processes orders. It has accumulated significant technical debt:
/// - Magic numbers everywhere
/// - Deeply nested conditionals
/// - Duplicated logic
/// - Dictionary-based data model instead of typed classes
/// - Static mutable state (cache) with no thread safety
/// - Console.WriteLine for logging (tight coupling)
/// - One god method that does everything
/// </summary>
public class OrderProcessor_Original
{
    // BUG: Static mutable dictionary with no synchronization -- race condition!
    private static Dictionary<string, Dictionary<string, string>> _orderCache = new();

    /// <summary>
    /// Processes an order represented as a dictionary of string key-value pairs.
    /// Expected keys: "OrderId", "CustomerName", "CustomerType", "Items", "Total"
    /// CustomerType: "regular", "premium", "vip"
    /// Items: semicolon-separated list like "Widget x2;Gadget x1"
    /// Total: decimal string like "149.99"
    /// 
    /// Returns a dictionary with: "OrderId", "OriginalTotal", "Discount", "DiscountedTotal",
    /// "Tax", "ShippingCost", "FinalTotal", "Status"
    /// </summary>
    public Dictionary<string, string> ProcessOrder(Dictionary<string, string> order)
    {
        var result = new Dictionary<string, string>();

        // Validate -- but in a messy way
        if (order == null)
        {
            Console.WriteLine("ERROR: Order is null!");
            throw new ArgumentNullException(nameof(order));
        }

        if (!order.ContainsKey("OrderId") || string.IsNullOrEmpty(order["OrderId"]))
        {
            Console.WriteLine("ERROR: Missing OrderId");
            throw new ArgumentException("OrderId is required");
        }

        if (!order.ContainsKey("CustomerName") || string.IsNullOrEmpty(order["CustomerName"]))
        {
            Console.WriteLine("ERROR: Missing CustomerName");
            throw new ArgumentException("CustomerName is required");
        }

        if (!order.ContainsKey("CustomerType") || string.IsNullOrEmpty(order["CustomerType"]))
        {
            Console.WriteLine("ERROR: Missing CustomerType");
            throw new ArgumentException("CustomerType is required");
        }

        if (!order.ContainsKey("Total") || string.IsNullOrEmpty(order["Total"]))
        {
            Console.WriteLine("ERROR: Missing Total");
            throw new ArgumentException("Total is required");
        }

        var orderId = order["OrderId"];
        var customerName = order["CustomerName"];
        var customerType = order["CustomerType"].ToLower();
        var totalStr = order["Total"];

        Console.WriteLine($"Processing order {orderId} for {customerName}...");

        decimal total;
        // Parse total from string -- fragile!
        if (!decimal.TryParse(totalStr, out total))
        {
            Console.WriteLine($"ERROR: Invalid total: {totalStr}");
            throw new FormatException($"Invalid total: {totalStr}");
        }

        if (total <= 0)
        {
            Console.WriteLine($"ERROR: Total must be positive: {total}");
            throw new ArgumentException("Total must be positive");
        }

        result["OrderId"] = orderId;
        result["OriginalTotal"] = total.ToString("F2");

        // Calculate discount -- deeply nested, duplicated logic
        decimal discountPercent = 0;
        decimal discountAmount = 0;

        if (customerType == "regular")
        {
            Console.WriteLine("  Customer type: Regular");
            if (total < 50)
            {
                // No discount for regular customers under $50
                discountPercent = 0;
                Console.WriteLine("  No discount applied (order under $50)");
            }
            else if (total < 100)
            {
                // 5% discount for regular customers $50-$99.99
                discountPercent = 0.05m;
                Console.WriteLine("  5% discount applied");
            }
            else
            {
                // 10% discount for regular customers $100+
                discountPercent = 0.10m;
                Console.WriteLine("  10% discount applied");
            }
        }
        else if (customerType == "premium")
        {
            Console.WriteLine("  Customer type: Premium");
            if (total < 100)
            {
                // 10% discount for premium customers under $100
                discountPercent = 0.10m;
                Console.WriteLine("  10% discount applied");
            }
            else if (total < 500)
            {
                // 15% discount for premium customers $100-$499.99
                discountPercent = 0.15m;
                Console.WriteLine("  15% discount applied");
            }
            else
            {
                // 20% discount for premium customers $500+
                discountPercent = 0.20m;
                Console.WriteLine("  20% discount applied");
            }
        }
        else if (customerType == "vip")
        {
            Console.WriteLine("  Customer type: VIP");
            // VIP always gets 15%
            discountPercent = 0.15m;
            Console.WriteLine("  15% VIP discount applied");

            // VIP bonus: extra 5% for orders >= $200
            if (total >= 200)
            {
                discountPercent = 0.15m + 0.05m; // 20% total
                Console.WriteLine("  Extra 5% VIP bonus applied (order >= $200)");
            }
        }
        else
        {
            Console.WriteLine($"  WARNING: Unknown customer type '{customerType}', no discount");
            discountPercent = 0;
        }

        discountAmount = total * discountPercent;
        var discountedTotal = total - discountAmount;

        result["Discount"] = discountAmount.ToString("F2");
        result["DiscountedTotal"] = discountedTotal.ToString("F2");

        Console.WriteLine($"  Discount: {discountPercent:P0} = -${discountAmount:F2}");
        Console.WriteLine($"  Subtotal after discount: ${discountedTotal:F2}");

        // Calculate tax -- magic number 0.08 (8%)
        var tax = discountedTotal * 0.08m;
        result["Tax"] = tax.ToString("F2");
        Console.WriteLine($"  Tax (8%): ${tax:F2}");

        // Calculate shipping -- magic number 50 threshold, 5.99 cost
        decimal shippingCost;
        if (discountedTotal >= 50)
        {
            shippingCost = 0;
            Console.WriteLine("  Shipping: FREE (order >= $50)");
        }
        else
        {
            shippingCost = 5.99m;
            Console.WriteLine($"  Shipping: ${shippingCost:F2}");
        }
        result["ShippingCost"] = shippingCost.ToString("F2");

        // Calculate final total
        var finalTotal = discountedTotal + tax + shippingCost;
        result["FinalTotal"] = finalTotal.ToString("F2");
        result["Status"] = "Processed";

        Console.WriteLine($"  Final total: ${finalTotal:F2}");
        Console.WriteLine($"  Order {orderId} processed successfully!");

        // Cache the result -- NOT THREAD SAFE!
        _orderCache[orderId] = result;

        // Also cache with customer name prefix -- duplicated caching logic
        _orderCache[$"{customerName}_{orderId}"] = result;

        return result;
    }

    /// <summary>
    /// Retrieves a cached order by ID. Returns null if not found.
    /// NOT THREAD SAFE -- reads from static dictionary without synchronization.
    /// </summary>
    public Dictionary<string, string>? GetCachedOrder(string orderId)
    {
        if (_orderCache.ContainsKey(orderId))
        {
            Console.WriteLine($"Cache hit for order {orderId}");
            return _orderCache[orderId];
        }
        Console.WriteLine($"Cache miss for order {orderId}");
        return null;
    }

    /// <summary>
    /// Processes multiple orders. Just loops and calls ProcessOrder.
    /// Exceptions in one order stop processing of remaining orders -- another bug.
    /// </summary>
    public List<Dictionary<string, string>> ProcessBulkOrders(List<Dictionary<string, string>> orders)
    {
        var results = new List<Dictionary<string, string>>();
        Console.WriteLine($"Processing {orders.Count} orders in bulk...");

        for (int i = 0; i < orders.Count; i++)
        {
            Console.WriteLine($"\n--- Order {i + 1} of {orders.Count} ---");
            var result = ProcessOrder(orders[i]);
            results.Add(result);
        }

        Console.WriteLine($"\nBulk processing complete. {results.Count}/{orders.Count} orders processed.");
        return results;
    }

    // Helper to clear the cache -- useful for testing but exposes internal state
    public static void ClearCache()
    {
        _orderCache.Clear();
    }
}
