namespace IX09_Refactoring_LegacyOrderSystem.DiscountStrategies;

public interface IDiscountStrategy
{
    decimal CalculateDiscount(decimal amount);
}

