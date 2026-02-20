namespace IX09_Refactoring_LegacyOrderSystem.Models;

public record DiscountTier
(
    decimal MinOrderTotal,
    decimal Percentage
);