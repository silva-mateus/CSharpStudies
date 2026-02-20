using FluentAssertions;
using IX09_Refactoring_LegacyOrderSystem;
using IX09_Refactoring_LegacyOrderSystem.Calculators;
using IX09_Refactoring_LegacyOrderSystem.DataAccess;
using IX09_Refactoring_LegacyOrderSystem.Models;
using System.Globalization;
using Xunit;

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
    private readonly OrderServices _processor;
    private const decimal globalTax = OrderConstants.TaxRate;


    public OrderProcessorCharacterizationTests()
    {
        // Clear static cache between tests to prevent interference
        var repository = new InMemoryOrderRepository();
        var shippingCalculator = new ShippingCalculator();
        var taxCalculator = new TaxCalculator();
        var logger = new NullLogger();

        _processor = new OrderServices(repository, shippingCalculator, taxCalculator, logger);
    }

    [Theory]
    [InlineData("regular", "49.00")]
    [InlineData("regular", "9.99")]
    [InlineData("regular", "5.49")]
    public void ProcessOrder_ShouldNotGiveDiscount_RegularCustomerWithOrderUnder50(string customerType, string total)
    {
        var order = GenerateOrder(customerType, total);
        var result = _processor.ProcessOrder(order);

        var orderTotal = ParseStringToDecimal(order["Total"]);

        var discount = 0m;

        var discountAmount = orderTotal * discount;
        var discountedTotal = orderTotal - discountAmount;

        var shipping = 0m;
        if (orderTotal < 50)
        {
            shipping = 5.99m;
        }

        var tax = discountedTotal * globalTax;

        var finalTotal = discountedTotal + tax + shipping;

        result["OrderId"].Should().Be("Test-000");
        result["Discount"].Should().Be("0.00");
        result["ShippingCost"].Should().Be(shipping.ToString("F2"));
        result["Tax"].Should().Be(tax.ToString("F2"));
        result["FinalTotal"].Should().Be(finalTotal.ToString("F2"));
        result["Status"].Should().Be("Processed");
    }

    [Theory]
    [InlineData("regular", "50.00")]
    [InlineData("regular", "98.99")]
    [InlineData("regular", "70.49")]
    public void ProcessOrder_ShouldGive5PercDiscount_RegularCustomerWithOrderGreaterThenOrEquals50AndLessThen100(string customerType, string total)
    {
        var order = GenerateOrder(customerType, total);
        var result = _processor.ProcessOrder(order);

        var orderTotal = ParseStringToDecimal(order["Total"]);
        var discount = GetDiscount(customerType, orderTotal);
        var discountAmount = orderTotal * discount;
        var discountedTotal = orderTotal - discountAmount;

        var shipping = 0m;
        if (discountedTotal < 50)
        {
            shipping = 5.99m;
        }

        var tax = discountedTotal * globalTax;

        var finalTotal = discountedTotal + tax + shipping;

        result["OrderId"].Should().Be("Test-000");
        result["Discount"].Should().Be(discountAmount.ToString("F2"));
        result["ShippingCost"].Should().Be(shipping.ToString("F2"));
        result["Tax"].Should().Be(tax.ToString("F2"));
        result["FinalTotal"].Should().Be(finalTotal.ToString("F2"));
        result["Status"].Should().Be("Processed");
    }

    [Theory]

    [InlineData("regular", "100.00")]
    [InlineData("regular", "250.00")]
    [InlineData("regular", "420.99")]
    public void ProcessOrder_ShouldGive10PercDiscount_RegularCustomerWithOrderBetweenGreaterThenOrEquals100(string customerType, string total)
    {
        var order = GenerateOrder(customerType, total);
        var result = _processor.ProcessOrder(order);

        var orderTotal = ParseStringToDecimal(order["Total"]);
        var discount = GetDiscount(customerType, orderTotal);
        var discountAmount = orderTotal * discount;
        var discountedTotal = orderTotal - discountAmount;

        var shipping = 0m;
        if (discountedTotal < 50)
        {
            shipping = 5.99m;
        }

        var tax = discountedTotal * globalTax;

        var finalTotal = discountedTotal + tax + shipping;

        result["OrderId"].Should().Be("Test-000");
        result["Discount"].Should().Be(discountAmount.ToString("F2"));
        result["ShippingCost"].Should().Be(shipping.ToString("F2"));
        result["Tax"].Should().Be(tax.ToString("F2"));
        result["FinalTotal"].Should().Be(finalTotal.ToString("F2"));
        result["Status"].Should().Be("Processed");
    }

    [Theory]
    [InlineData("premium", "50.00")]
    [InlineData("premium", "99.99")]
    [InlineData("premium", "70.49")]
    public void ProcessOrder_ShouldGive10PercDiscount_PremiumCustomerWithOrderLessThen100(string customerType, string total)
    {
        var order = GenerateOrder(customerType, total);
        var result = _processor.ProcessOrder(order);

        var orderTotal = ParseStringToDecimal(order["Total"]);
        var discount = GetDiscount(customerType, orderTotal);
        var discountAmount = orderTotal * discount;
        var discountedTotal = orderTotal - discountAmount;

        var shipping = 0m;
        if (discountedTotal < 50)
        {
            shipping = 5.99m;
        }

        var tax = discountedTotal * globalTax;

        var finalTotal = discountedTotal + tax + shipping;

        result["OrderId"].Should().Be("Test-000");
        result["Discount"].Should().Be(discountAmount.ToString("F2"));
        result["ShippingCost"].Should().Be(shipping.ToString("F2"));
        result["Tax"].Should().Be(tax.ToString("F2"));
        result["FinalTotal"].Should().Be(finalTotal.ToString("F2"));
        result["Status"].Should().Be("Processed");
    }

    [Theory]
    [InlineData("premium", "499.99")]
    [InlineData("premium", "100.00")]
    [InlineData("premium", "259.59")]
    public void ProcessOrder_ShouldGive15PercDiscount_PremiumCustomerWithOrderGreaterThenOrEqual100LessThen500(string customerType, string total)
    {
        var order = GenerateOrder(customerType, total);
        var result = _processor.ProcessOrder(order);

        var orderTotal = ParseStringToDecimal(order["Total"]);
        var discount = GetDiscount(customerType, orderTotal);
        var discountAmount = orderTotal * discount;
        var discountedTotal = orderTotal - discountAmount;

        var shipping = 0m;
        if (discountedTotal < 50)
        {
            shipping = 5.99m;
        }

        var tax = discountedTotal * globalTax;

        var finalTotal = discountedTotal + tax + shipping;

        result["OrderId"].Should().Be("Test-000");
        result["Discount"].Should().Be(discountAmount.ToString("F2"));
        result["ShippingCost"].Should().Be(shipping.ToString("F2"));
        result["Tax"].Should().Be(tax.ToString("F2"));
        result["FinalTotal"].Should().Be(finalTotal.ToString("F2"));
        result["Status"].Should().Be("Processed");
    }

    [Theory]
    [InlineData("premium", "500.00")]
    [InlineData("premium", "999.99")]
    [InlineData("premium", "704.49")]
    public void ProcessOrder_ShouldGive20PercDiscount_PremiumCustomerWithOrderGreaterThenOrEqual500(string customerType, string total)
    {
        var order = GenerateOrder(customerType, total);
        var result = _processor.ProcessOrder(order);

        var orderTotal = ParseStringToDecimal(order["Total"]);
        var discount = GetDiscount(customerType, orderTotal);
        var discountAmount = orderTotal * discount;
        var discountedTotal = orderTotal - discountAmount;

        var shipping = 0m;
        if (discountedTotal < 50)
        {
            shipping = 5.99m;
        }

        var tax = discountedTotal * globalTax;

        var finalTotal = discountedTotal + tax + shipping;

        result["OrderId"].Should().Be("Test-000");
        result["Discount"].Should().Be(discountAmount.ToString("F2"));
        result["ShippingCost"].Should().Be(shipping.ToString("F2"));
        result["Tax"].Should().Be(tax.ToString("F2"));
        result["FinalTotal"].Should().Be(finalTotal.ToString("F2"));
        result["Status"].Should().Be("Processed");
    }

    [Theory]
    [InlineData("vip", "199.99")]
    [InlineData("vip", "99.99")]
    [InlineData("vip", "5.49")]
    public void ProcessOrder_ShouldGive15PercDiscount_VipCustomerWithOrderLessThen200(string customerType, string total)
    {
        var order = GenerateOrder(customerType, total);
        var result = _processor.ProcessOrder(order);

        var orderTotal = ParseStringToDecimal(order["Total"]);
        var discount = GetDiscount(customerType, orderTotal);
        var discountAmount = orderTotal * discount;
        var discountedTotal = orderTotal - discountAmount;

        var shipping = 0m;
        if (discountedTotal < 50)
        {
            shipping = 5.99m;
        }

        var tax = discountedTotal * globalTax;

        var finalTotal = discountedTotal + tax + shipping;

        result["OrderId"].Should().Be("Test-000");
        result["Discount"].Should().Be(discountAmount.ToString("F2"));
        result["ShippingCost"].Should().Be(shipping.ToString("F2"));
        result["Tax"].Should().Be(tax.ToString("F2"));
        result["FinalTotal"].Should().Be(finalTotal.ToString("F2"));
        result["Status"].Should().Be("Processed");
    }

    [Theory]
    [InlineData("vip", "200.00")]
    [InlineData("vip", "999.99")]
    [InlineData("vip", "704.49")]
    public void ProcessOrder_ShouldGive20PercDiscount_VipCustomerWithOrderGreaterThenOrEqual200(string customerType, string total)
    {
        var order = GenerateOrder(customerType, total);
        var result = _processor.ProcessOrder(order);

        var orderTotal = ParseStringToDecimal(order["Total"]);
        var discount = GetDiscount(customerType, orderTotal);
        var discountAmount = orderTotal * discount;
        var discountedTotal = orderTotal - discountAmount;

        var shipping = 0m;
        if (discountedTotal < 50)
        {
            shipping = 5.99m;
        }

        var tax = discountedTotal * globalTax;

        var finalTotal = discountedTotal + tax + shipping;

        result["OrderId"].Should().Be("Test-000");
        result["Discount"].Should().Be(discountAmount.ToString("F2"));
        result["ShippingCost"].Should().Be(shipping.ToString("F2"));
        result["Tax"].Should().Be(tax.ToString("F2"));
        result["FinalTotal"].Should().Be(finalTotal.ToString("F2"));
        result["Status"].Should().Be("Processed");
    }


    [Theory]
    [InlineData("regular", "100.00")]
    [InlineData("premium", "199.99")]
    [InlineData("vip", "250.49")]
    public void ProcessOrder_ShippingShouldBeFree_AnyCustomerWithOrderDiscountedTotalGreaterThenOrEquals50(string customerType, string total)
    {
        var order = GenerateOrder(customerType, total);
        var result = _processor.ProcessOrder(order);

        var orderTotal = ParseStringToDecimal(order["Total"]);

        var discount = GetDiscount(customerType, orderTotal);
        var discountAmount = orderTotal * discount;
        var discountedTotal = orderTotal - discountAmount;

        var shipping = 0m;
        if (discountedTotal < 50)
        {
            shipping = 5.99m;
        }

        var tax = discountedTotal * globalTax;

        var finalTotal = discountedTotal + tax + shipping;

        result["ShippingCost"].Should().Be("0.00");
    }

    [Theory]
    [InlineData("regular", "49.99")]
    [InlineData("premium", "9.99")]
    [InlineData("vip", "5.49")]
    public void ProcessOrder_ShippingShouldBe5_99_WhenOrderTotalAfterDiscountLessThen50(string customerType, string total)
    {
        var order = GenerateOrder(customerType, total);
        var result = _processor.ProcessOrder(order);

        var orderTotal = ParseStringToDecimal(order["Total"]);

        var discount = GetDiscount(customerType, orderTotal);
        var discountAmount = orderTotal * discount;
        var discountedTotal = orderTotal - discountAmount;


        var shipping = 0m;
        if (discountedTotal < 50)
        {
            shipping = 5.99m;
        }

        var tax = discountedTotal * globalTax;

        var finalTotal = discountedTotal + tax + shipping;

        result["ShippingCost"].Should().Be("5.99");
    }

    [Theory]
    [InlineData("regular", "49.99")]
    [InlineData("regular", "70.00")]
    [InlineData("regular", "150.00")]
    [InlineData("premium", "9.99")]
    [InlineData("premium", "199.99")]
    [InlineData("premium", "599.99")]
    [InlineData("vip", "5.49")]
    [InlineData("vip", "255.49")]
    public void ProcessOrder_TaxShouldBe8PercOfPostDiscountTotal_AnyCustomerAnyOrderValue(string customerType, string total)
    {
        var order = GenerateOrder(customerType, total);
        var result = _processor.ProcessOrder(order);

        var orderTotal = ParseStringToDecimal(order["Total"]);

        var discount = GetDiscount(customerType, orderTotal);
        var discountAmount = orderTotal * discount;
        var discountedTotal = orderTotal - discountAmount;

        var shipping = 0m;
        if (discountedTotal < 50)
        {
            shipping = 5.99m;
        }

        var tax = discountedTotal * globalTax;

        var finalTotal = discountedTotal + tax + shipping;

        result["Tax"].Should().Be(tax.ToString("F2"));
    }

    [Fact]
    public void ProcessOrder_ShouldThrow_MissingOrderId()
    {
        var order = new Dictionary<string, string>
        {
            ["CustomerName"] = "Test Customer",
            ["CustomerType"] = "regular",
            ["Items"] = "Test Item x1",
            ["Total"] = "59.99"
        };

        Action action = () => _processor.ProcessOrder(order);
        action.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void ProcessOrder_ShouldThrow_MissingCustomerName()
    {
        var order = new Dictionary<string, string>
        {
            ["OrderId"] = "Test-000",
            ["CustomerType"] = "regular",
            ["Items"] = "Test Item x1",
            ["Total"] = "59.99"
        };

        Action action = () => _processor.ProcessOrder(order);
        action.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void ProcessOrder_ShouldThrow_MissingCustomerType()
    {
        var order = new Dictionary<string, string>
        {
            ["OrderId"] = "Test-000",
            ["CustomerName"] = "Test Customer",
            ["Items"] = "Test Item x1",
            ["Total"] = "59.99"
        };

        Action action = () => _processor.ProcessOrder(order);
        action.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void ProcessOrder_ShouldThrow_MissingTotal()
    {
        var order = new Dictionary<string, string>
        {
            ["OrderId"] = "Test-000",
            ["CustomerName"] = "Test Customer",
            ["CustomerType"] = "regular",
            ["Items"] = "Test Item x1",
        };

        Action action = () => _processor.ProcessOrder(order);
        action.Should().Throw<ArgumentException>();
    }

    /// 16. Negative total throws ArgumentException.
    /// 

    [Fact]
    public void ProcessOrder_ShouldThrowArgumentNullException_OnNullOrder()
    {
        Action action = () => _processor.ProcessOrder(null!);
        action.Should().Throw<ArgumentNullException>();
    }

    [Theory]
    [InlineData ("14%99")]
    [InlineData ("1A99")]
    public void ProcessOrder_ShouldThrowFormatException_OnInvalidTotal(string totalStr)
    {
        var order = new Dictionary<string, string>
        {
            ["OrderId"] = "Test-000",
            ["CustomerName"] = "Test Customer",
            ["CustomerType"] = "regular",
            ["Items"] = "Test Item x1",
            ["Total"] = totalStr
        };

        Action action = () => _processor.ProcessOrder(order);
        action.Should().Throw<FormatException>();
    }

    [Fact]
    public void ProcessOrder_ShouldThrowArgumentException_OnNegativeTotal()
    {
        var order = new Dictionary<string, string>
        {
            ["OrderId"] = "Test-000",
            ["CustomerName"] = "Test Customer",
            ["CustomerType"] = "regular",
            ["Items"] = "Test Item x1",
            ["Total"] = "-99.99"
        };

        Action action = () => _processor.ProcessOrder(order);
        action.Should().Throw<ArgumentException>();
    }

    private Dictionary<string, string> GenerateOrder(string customerType, string total)
    {
        return new Dictionary<string, string>
        {
            ["OrderId"] = "Test-000",
            ["CustomerName"] = "Test Customer",
            ["CustomerType"] = customerType,
            ["Items"] = "Test Item x1",
            ["Total"] = total
        };
    }

    private decimal ParseStringToDecimal(string numberStr)
    {
        if (!decimal.TryParse(
                numberStr.Trim(),
                NumberStyles.Number,
                CultureInfo.InvariantCulture,
                out var numberDecimal))
        {
            throw new FormatException($"Invalid number String: {numberStr}");
        }
        return numberDecimal;
    }

    private decimal GetDiscount(string customerType, decimal total)
    {
        if (customerType == "regular")
        {
            if (total < 50)
                return 0m;
            else if (total < 100)
                return 0.05m;
            else
                return 0.10m;
        }
        else if (customerType == "premium")
        {
            if (total < 100)
                return 0.10m;
            else if (total < 500)
                return 0.15m;
            else
                return 0.20m;
        }
        else if (customerType == "vip")
        {
            if (total >= 200)
                return 0.20m;
            else
                return 0.15m;
        }

        throw new ArgumentException($"Invalid customer type: {customerType}");
    }
}
