namespace Session14.Tests;

using FibonacciGenerator;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;

[TestFixture]
public class FibonacciTests
{
    [TestCase(1, new int[] { 0 })]
    [TestCase(2, new int[] { 0, 1 })]
    [TestCase(5, new int[] { 0, 1, 1, 2, 3 })]
    public void Generate_ValidInput_ReturnsList(int number, int[] expected)
    {
        var result = Fibonacci.Generate(number);

        Assert.That(result, Is.EqualTo(expected));

    }

    [Test]
    public void Generate_InputZero_ReturnsEmpty()
    {
        var result = Fibonacci.Generate(0);

        Assert.That(result, Is.Empty);
    }

    [Test]
    public void Generate_Input_46_LastNumberInListIs_1836311903()
    {
        var result = Fibonacci.Generate(46);

        int lastNumber = result.Last();

        Assert.That(lastNumber, Is.EqualTo(1134903170));
    }

    [TestCase(-1)]
    [TestCase(-100)]
    [TestCase(-2000)]
    [TestCase(47)]
    [TestCase(100)]
    [TestCase(1000)]
    public void Generate_OutOfBoundariesInput_ReturnsException(int number)
    {
        Assert.That(() => Fibonacci.Generate(number), Throws.ArgumentException);
    }

}