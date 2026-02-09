namespace Assignment_Password_Generator_Refactoring;

public interface IRandomGenerator
{
    int Next(int maxValue);
    int Next(int minValue, int maxValue);
}