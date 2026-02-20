namespace IX09_Refactoring_LegacyOrderSystem.Models;

public record Order
(
    string OrderId,
    Customer Customer,
    List<string> Items,
    decimal Total
);

