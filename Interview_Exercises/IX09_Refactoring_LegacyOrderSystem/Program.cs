using IX09_Refactoring_LegacyOrderSystem;
using IX09_Refactoring_LegacyOrderSystem.Calculators;
using IX09_Refactoring_LegacyOrderSystem.DataAccess;
using IX09_Refactoring_LegacyOrderSystem.Logger;

var orderRepository = new InMemoryOrderRepository();
var shippingCalculator = new ShippingCalculator();
var taxCalculator = new TaxCalculator();
var logger = new ConsoleLogger();

var processor = new OrderServices(orderRepository, shippingCalculator, taxCalculator, logger);

// Sample order 1: Regular customer, $75 order
var order1 = new Dictionary<string, string>
{
    ["OrderId"] = "ORD-001",
    ["CustomerName"] = "John Doe",
    ["CustomerType"] = "regular",
    ["Items"] = "Widget x2;Gadget x1",
    ["Total"] = "49.00"
};

// Sample order 2: Premium customer, $250 order
var order2 = new Dictionary<string, string>
{
    ["OrderId"] = "ORD-002",
    ["CustomerName"] = "Jane Smith",
    ["CustomerType"] = "premium",
    ["Items"] = "Deluxe Widget x5",
    ["Total"] = "50.00"
};

// Sample order 3: VIP customer, $500 order
var order3 = new Dictionary<string, string>
{
    ["OrderId"] = "ORD-003",
    ["CustomerName"] = "Bob VIP",
    ["CustomerType"] = "vip",
    ["Items"] = "Premium Pack x1",
    ["Total"] = "500.00"
};

Console.WriteLine("========================================");
var result1 = processor.ProcessOrder(order1);
Console.WriteLine();

Console.WriteLine("========================================");
var result2 = processor.ProcessOrder(order2);
Console.WriteLine();

Console.WriteLine("========================================");
var result3 = processor.ProcessOrder(order3);
Console.WriteLine();

// Print summaries
Console.WriteLine("========================================");
Console.WriteLine("ORDER SUMMARIES:");
Console.WriteLine($"  {result1["OrderId"]}: Original=${result1["OriginalTotal"]}, " +
    $"Discount=-${result1["Discount"]}, Tax=${result1["Tax"]}, " +
    $"Shipping=${result1["ShippingCost"]}, Final=${result1["FinalTotal"]}");
Console.WriteLine($"  {result2["OrderId"]}: Original=${result2["OriginalTotal"]}, " +
    $"Discount=-${result2["Discount"]}, Tax=${result2["Tax"]}, " +
    $"Shipping=${result2["ShippingCost"]}, Final=${result2["FinalTotal"]}");
Console.WriteLine($"  {result3["OrderId"]}: Original=${result3["OriginalTotal"]}, " +
    $"Discount=-${result3["Discount"]}, Tax=${result3["Tax"]}, " +
    $"Shipping=${result3["ShippingCost"]}, Final=${result3["FinalTotal"]}");
