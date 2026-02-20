using IX09_Refactoring_LegacyOrderSystem.Models;

namespace IX09_Refactoring_LegacyOrderSystem.Calculators;

public class TaxCalculator : ITaxCalculator
{
    public decimal Calculate(decimal amount)
    {
        return amount * OrderConstants.TaxRate;
    }
}
