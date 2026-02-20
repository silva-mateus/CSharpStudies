using IX09_Refactoring_LegacyOrderSystem.Models;

namespace IX09_Refactoring_LegacyOrderSystem.DiscountStrategies;

public class RegularCustomerDiscount : IDiscountStrategy
{
    private readonly List<DiscountTier> _discountTierList;

    public RegularCustomerDiscount()
    {
        _discountTierList = new List<DiscountTier>
        {
            new DiscountTier(0, 0m),
            new DiscountTier(50, 0.05m),
            new DiscountTier(100, 0.10m)
        };
    }

    public decimal CalculateDiscount(decimal amount)
    {
        var applicable = _discountTierList
            .Where(d => d.MinOrderTotal <= amount)
            .MaxBy(d => d.MinOrderTotal);

        return applicable?.Percentage * amount ?? 0m;
    }
}