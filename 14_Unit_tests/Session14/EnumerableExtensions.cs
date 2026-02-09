namespace Session14;

public static class EnumerableExtensions
{
    public static int SumOfEvenNumbers(this IEnumerable<int> numbers)
    {
        return numbers.Where(n => n % 2 == 0).Sum();
    }

}
