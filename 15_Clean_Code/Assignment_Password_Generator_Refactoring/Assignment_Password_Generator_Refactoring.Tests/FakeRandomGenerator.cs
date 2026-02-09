namespace Assignment_Password_Generator_Refactoring.Tests;

public class FakeRandomGenerator : IRandomGenerator
{
    private readonly Queue<int> _values;

    public FakeRandomGenerator(params int[] values)
    {
        _values = new Queue<int>(values);
    }

    public int Next(int maxValue) => _values.Dequeue();
    public int Next(int minValue, int maxValue) => _values.Dequeue();
}