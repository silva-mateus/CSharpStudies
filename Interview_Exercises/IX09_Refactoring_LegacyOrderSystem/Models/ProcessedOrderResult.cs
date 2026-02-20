namespace IX09_Refactoring_LegacyOrderSystem.Models;


public enum OrderResultStatus
{
    Processed
}

public record ProcessedOrderResult
(
    string OrderId,
    decimal OriginalTotal,
    decimal Discount,
    decimal DiscountedTotal,
    decimal Tax,
    decimal ShippingCost,
    decimal FinalTotal,
    OrderResultStatus Status
);
