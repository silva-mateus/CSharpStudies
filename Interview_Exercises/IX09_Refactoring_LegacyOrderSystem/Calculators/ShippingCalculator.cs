using IX09_Refactoring_LegacyOrderSystem.Models;

namespace IX09_Refactoring_LegacyOrderSystem.Calculators;

public class ShippingCalculator : IShippingCalculator
{
    public decimal Calculate(decimal orderTotal)
    {
        return orderTotal >= OrderConstants.FreeShippingThreshold
            ? 0
            : OrderConstants.ShippingCost;
    }
}
