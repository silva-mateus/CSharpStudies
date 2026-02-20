using IX09_Refactoring_LegacyOrderSystem.Calculators;
using IX09_Refactoring_LegacyOrderSystem.DataAccess;
using IX09_Refactoring_LegacyOrderSystem.DiscountStrategies;
using IX09_Refactoring_LegacyOrderSystem.Logger;
using IX09_Refactoring_LegacyOrderSystem.Models;
using System.Globalization;

namespace IX09_Refactoring_LegacyOrderSystem;

public class OrderServices
{
    private readonly IOrderRepository _orderRepository;
    private readonly IShippingCalculator _shippingCalculator;
    private readonly ITaxCalculator _taxCalculator;
    private readonly ILogger _logger;

    public OrderServices(IOrderRepository orderRepository, IShippingCalculator shippingCalculator, ITaxCalculator taxCalculator, ILogger logger)
    {
        _orderRepository = orderRepository;
        _shippingCalculator = shippingCalculator;
        _taxCalculator = taxCalculator;
        _logger = logger;
    }

    public Dictionary<string, string> ProcessOrder(Dictionary<string, string> orderDict)
    {
        Order order;
        IDiscountStrategy discountStrategy;
        try
        {
            order = CreateOrder(orderDict);
            discountStrategy = GetDiscountStrategy(order.Customer.CustomerType);

        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex);
            throw;
        }

        _logger.LogInfo($"Processing order {order.OrderId} for {order.Customer.CustomerName}...");

        var discount = discountStrategy.CalculateDiscount(order.Total);
        var discountedTotal = order.Total - discount;

        var tax = _taxCalculator.Calculate(discountedTotal);

        var shippingCost = _shippingCalculator.Calculate(discountedTotal);

        var finalTotal = discountedTotal + tax + shippingCost;

        var result = new ProcessedOrderResult(
            order.OrderId,
            order.Total,
            discount,
            discountedTotal,
            tax,
            shippingCost,
            finalTotal,
            OrderResultStatus.Processed);

        _orderRepository.Save(order.OrderId, result);

        return GenerateProcessedOrderDict(result);
    }

    private static IDiscountStrategy GetDiscountStrategy(CustomerType customerType)
    {
        return customerType switch
        {
            CustomerType.Regular => new RegularCustomerDiscount(),
            CustomerType.Premium => new PremiumCustomerDiscount(),
            CustomerType.VIP => new VipCustomerDiscount(),
            _ => throw new ArgumentException($"Invalid customer type: {customerType}")
        };
    }

    private Order CreateOrder(Dictionary<string, string> orderDict)
    {
        if (orderDict == null)
            throw new ArgumentNullException(nameof(orderDict));

        var orderId = GetRequiredValue("OrderId", orderDict);
        var customerName = GetRequiredValue("CustomerName", orderDict);
        var customerTypeStr = GetRequiredValue("CustomerType", orderDict);
        var totalStr = GetRequiredValue("Total", orderDict);

        if (!Enum.TryParse<CustomerType>(customerTypeStr, ignoreCase: true, out var customerType))
            throw new ArgumentException($"Unknown customer type: {customerTypeStr}");

        if (!decimal.TryParse(totalStr, NumberStyles.Number, CultureInfo.InvariantCulture, out var total))
            throw new FormatException($"Invalid total: {totalStr}");

        if (total <= 0)
            throw new ArgumentException("Total must be positive");


        orderDict.TryGetValue("Items", out var itemsStr);
        var items = string.IsNullOrEmpty(itemsStr) ? new List<string>() : itemsStr.Split(';').ToList();

        return new Order(orderId, new Customer(customerName, customerType), items, total);

    }

    private string GetRequiredValue(string key, Dictionary<string, string> dict)
    {
        if (!dict.TryGetValue(key, out var value) || string.IsNullOrEmpty(value))
            throw new ArgumentException($"{key} is required");
        return value;
    }

    private Dictionary<string, string> GenerateProcessedOrderDict(ProcessedOrderResult result)
    {
        return new Dictionary<string, string>
        {
            ["OrderId"] = result.OrderId,
            ["OriginalTotal"] = result.OriginalTotal.ToString("F2"),
            ["Discount"] = result.Discount.ToString("F2"),
            ["DiscountedTotal"] = result.DiscountedTotal.ToString("F2"),
            ["Tax"] = result.Tax.ToString("F2"),
            ["ShippingCost"] = result.ShippingCost.ToString("F2"),
            ["FinalTotal"] = result.FinalTotal.ToString("F2"),
            ["Status"] = result.Status.ToString()
        };
    }


}
