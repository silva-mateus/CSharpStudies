namespace IX09_Refactoring_LegacyOrderSystem.Calculators;

public interface IShippingCalculator
{
    public decimal Calculate(decimal orderTotal);
}
