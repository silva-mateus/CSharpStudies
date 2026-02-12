using IX09_Refactoring_LegacyOrderSystem;

namespace IX09_Refactoring_LegacyOrderSystem.Tests;

/// <summary>
/// STEP 1: Write characterization tests BEFORE refactoring.
///
/// These tests document the current behavior of OrderProcessor.ProcessOrder().
/// Use the existing OrderProcessor directly. After refactoring, update these tests
/// to use the new OrderService (or keep a separate test class for the new code).
///
/// Helper: Use this method to create test orders:
///
///     private Dictionary&lt;string, string&gt; CreateOrder(
///         string orderId, string customerName, string customerType, string total)
///         =&gt; new()
///         {
///             ["OrderId"] = orderId,
///             ["CustomerName"] = customerName,
///             ["CustomerType"] = customerType,
///             ["Items"] = "TestItem x1",
///             ["Total"] = total
///         };
///
/// CHARACTERIZATION TESTS TO WRITE:
///
/// Discount logic:
///  1. Regular customer, total $30 -> 0% discount -> DiscountedTotal = $30.00
///  2. Regular customer, total $75 -> 5% discount -> DiscountedTotal = $71.25
///  3. Regular customer, total $150 -> 10% discount -> DiscountedTotal = $135.00
///  4. Premium customer, total $80 -> 10% discount -> DiscountedTotal = $72.00
///  5. Premium customer, total $250 -> 15% discount -> DiscountedTotal = $212.50
///  6. Premium customer, total $600 -> 20% discount -> DiscountedTotal = $480.00
///  7. VIP customer, total $100 -> 15% discount -> DiscountedTotal = $85.00
///  8. VIP customer, total $300 -> 20% discount (15% + 5% bonus) -> DiscountedTotal = $240.00
///
/// Tax (8% of discounted total):
///  9. Total $100, regular customer -> discount $10 -> subtotal $90 -> tax $7.20
///
/// Shipping:
/// 10. Discounted total >= $50 -> shipping $0.00
/// 11. Discounted total < $50 -> shipping $5.99
///     (e.g., regular customer, total $30, no discount -> $30 + $2.40 tax + $5.99 shipping)
///
/// Validation:
/// 12. Null order throws ArgumentNullException.
/// 13. Missing OrderId throws ArgumentException.
/// 14. Missing CustomerType throws ArgumentException.
/// 15. Invalid total string throws FormatException.
/// 16. Negative total throws ArgumentException.
///
/// IMPORTANT: Call OrderProcessor.ClearCache() in a constructor or setup method
/// to avoid test interference from the static cache.
/// </summary>
public class OrderProcessorCharacterizationTests
{
    public OrderProcessorCharacterizationTests()
    {
        // Clear static cache between tests to prevent interference
        OrderProcessor.ClearCache();
    }

    // TODO: Write your characterization tests here
}
