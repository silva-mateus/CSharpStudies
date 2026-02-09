namespace Session14.Tests;

public class EnumerableExtensionsTests
{
    [Test]
    public void SumOfEvenNumbers_EmptyInput_Returns0()
    {
        var input = Enumerable.Empty<int>();

        var result = input.SumOfEvenNumbers();

        Assert.That(result, Is.EqualTo(0));
    }

    [TestCase(8)]
    [TestCase(-8)]
    [TestCase(6)]
    [TestCase(0)]
    public void SumOfEvenNumbers_SingleEvenNumber_ReturnsEvenNumber(int evenNumber)
    {
        var input = new[] { evenNumber };
        var result = input.SumOfEvenNumbers();

        Assert.That(result, Is.EqualTo(evenNumber));
    }

    [TestCase(1)]
    [TestCase(3)]
    [TestCase(-5)]
    public void SumOfEvenNumbers_SingleOddNumber_Returns0(int oddNumber)
    {
        var input = new[] { oddNumber };
        var result = input.SumOfEvenNumbers();

        Assert.That(result, Is.EqualTo(0));
    }

    // [TestCase(new[] { 1, 3, 5 }, 0)]
    // [TestCase(new List<int> { 2, 4, 6 }, 12)]
    // [TestCase(new[] { 1, 2, 3, 4, 5, 6 }, 12)]
    // [TestCase(new[] { -2, -4, -6 }, -12)]
    // [TestCase(new[] { 0, 2, 4, 6, 8 }, 20)]
    [TestCaseSource(nameof(GetNumbers))]
    public void SumOfEvenNumbers_MultipleNumbers_ReturnsSumOfEvenNumbers(IEnumerable<int> numbers, int expected)
    {
        var result = numbers.SumOfEvenNumbers();
        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void SumOfEvenNumbers_NullInput_ThrowsArgumentNullException()
    {
        IEnumerable<int>? nullInput = null;
        Assert.That(() => nullInput!.SumOfEvenNumbers(), Throws.ArgumentNullException);
    }

    private static IEnumerable<object> GetNumbers()
    {
        return new[]
        {
            new object[] { new[] { 1, 3, 5 }, 0 },
            new object[] { new List<int> { 2, 4, 6 }, 12 },
            new object[] { new[] { 1, 2, 3, 4, 5, 6 }, 12 },
            new object[] { new[] { -2, -4, -6 }, -12 },
            new object[] { new[] { 0, 2, 4, 6, 8 }, 20 }
        };
    }

}
