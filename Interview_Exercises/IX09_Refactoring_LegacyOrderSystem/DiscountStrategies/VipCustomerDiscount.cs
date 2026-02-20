using IX09_Refactoring_LegacyOrderSystem.Models;
namespace IX09_Refactoring_LegacyOrderSystem.DiscountStrategies;

public class VipCustomerDiscount : IDiscountStrategy
{
    private readonly List<DiscountTier> _discountTierList;

    public VipCustomerDiscount()
    {
        _discountTierList = new List<DiscountTier>
        {
            new DiscountTier(0, 0.15m),
            new DiscountTier(200, 0.20m)
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