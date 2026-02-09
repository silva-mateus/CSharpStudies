namespace Assignment_Password_Generator_Refactoring;

public class SystemRandomGenerator : IRandomGenerator
{
    private readonly Random _random = new Random();

    public int Next(int maxValue) => _random.Next(maxValue);
    public int Next(int minValue, int maxValue) => _random.Next(minValue, maxValue);
}