using FluentAssertions;
using System.Collections;
using Xunit;
using Xunit.Abstractions;

namespace JD01_xUnit_Advanced_Testing.Tests;

/// <summary>
/// TODO: Write comprehensive tests for StringCalculator.
///
/// Requirements:
/// - Use nested classes to group tests (AddTests, MultiplyTests, ParseExpressionTests)
/// - Use [Fact] for simple single-case tests
/// - Use [Theory] + [InlineData] for parameterized Add tests
/// - Use [Theory] + [MemberData] for parameterized Multiply tests
/// - Use [Theory] + [ClassData] for parameterized ParseExpression tests
/// - Use ITestOutputHelper to log at least one diagnostic message
/// - Use both Assert.Throws and FluentAssertions .Should().Throw() syntax
/// - Use IClassFixture for GetCallerCount/Reset tests (shared instance)
///
/// MINIMUM 12 TESTS for StringCalculator:
///  1. Add("") returns 0
///  2. Add("5") returns 5
///  3. Add("1,2") returns 3
///  4. Add("1\n2\n3") returns 6
///  5. Add("-1,2") throws ArgumentException containing "Negatives not allowed"
///  6. [Theory] Add with multiple inputs: ("1,2,3", 6), ("10,20", 30), ("0,0", 0)
///  7. Multiply("") returns 1
///  8. [Theory+MemberData] Multiply with various inputs
///  9. [Theory+ClassData] ParseExpression with all 4 operators
/// 10. ParseExpression("10 / 0") throws DivideByZeroException
/// 11. ParseExpression("invalid") throws FormatException
/// 12. GetCallerCount tracks Add calls and resets
/// </summary>
public class StringCalculatorTests
{
    // TODO: your tests go here
    // Remember to use nested classes, [Theory], [InlineData], [MemberData], [ClassData],
    // ITestOutputHelper, IClassFixture, and FluentAssertions.

    public class AddTests
    {
        private readonly ITestOutputHelper _output;
        private readonly StringCalculator _calculator = new();

        public AddTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void EmptyString_ReturnsZero()
        {
            var result = _calculator.Add("");

            _output.WriteLine($"Result of Add('') = {result}");

            Assert.Equal(0, result);
        }

        [Theory]
        [InlineData("5", 5)]
        [InlineData("3", 3)]
        [InlineData("0", 0)]
        public void SingleNumber_ReturnsSameNumber(string numbers, int expectedResult)
        {
            var result = _calculator.Add(numbers);

            _output.WriteLine($"Result of Add({numbers}) = {expectedResult}");

            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData("1,2", 3)]
        [InlineData("1,2,3", 6)]
        [InlineData("1,2,3,4", 10)]
        public void CommaSeparatedNumbers_ReturnsExpectedResult(string numbers, int expectedResult)
        {
            var result = _calculator.Add(numbers);

            _output.WriteLine($"Result of Add({numbers}) = {expectedResult}");

            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData("1\n2", 3)]
        [InlineData("1\n2\n3", 6)]
        [InlineData("1\n2\n3\n4", 10)]
        public void NewLineSeparatedNumbers_ReturnsExpectedResult(string numbers, int expectedResult)
        {
            var result = _calculator.Add(numbers);

            _output.WriteLine($"Result of Add({numbers}) = {expectedResult}");

            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void NegativeNumbers_ThrowsArgumentException()
        {
            Action action = () => _calculator.Add("1,-1");

            var ex = Assert.Throws<ArgumentException>(action);
            Assert.Contains("Negatives not allowed", ex.Message);

            _output.WriteLine($"Adding negative numbers raises error with message: {ex.Message}");
        }
    }

    public class MultiplyTests // With FluentAssertions
    {
        private readonly ITestOutputHelper _output;
        private readonly StringCalculator _calculator = new();

        public MultiplyTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void EmptyString_Returns1()
        {
            var result = _calculator.Multiply("");
            result.Should().Be(1);
        }

        public static IEnumerable<object[]> MultiplyTestData()
        {
            yield return new object[] { "", 1 };
            yield return new object[] { "1,2", 2 };
            yield return new object[] { "1,2,3", 6 };
            yield return new object[] { "2,3,4", 24 };
            yield return new object[] { "4\n5", 20 };
            yield return new object[] { "4\n5\n6", 120 };
        }

        [Theory]
        [MemberData(nameof(MultiplyTestData))]
        public void ReturnsExpectedProduct(string input, int expected)
        {
            var result = _calculator.Multiply(input);
            _output.WriteLine($"Result of Multiply({input}) = {result}");
            result.Should().Be(expected);
        }

    }

    public class ParseExpressionTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { "3 + 4", 7.0 };
            yield return new object[] { "4 - 3", 1.0 };
            yield return new object[] { "9 * 9", 81.0 };
            yield return new object[] { "27 / 3", 9.0 };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    public class ParseExpressionTests
    {
        private readonly ITestOutputHelper _output;
        private readonly StringCalculator _calculator = new();

        public ParseExpressionTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Theory]
        [ClassData(typeof(ParseExpressionTestData))]
        public void ReturnsExpectedResult(string expression, double expected)
        {
            var result = _calculator.ParseExpression(expression);

            _output.WriteLine($"Result of ParseExpression({expression}) = {result}");
            result.Should().Be(expected);
        }

        [Fact]
        public void DivisionByZero_ThrowsDivideByZeroException()
        {
            Action action = () => _calculator.ParseExpression("1 / 0");
            action.Should().Throw<DivideByZeroException>();
        }

        [Theory]
        [InlineData("")]
        [InlineData("1+1")]
        [InlineData("1 + 1 + 1")]
        [InlineData("a * b")]
        [InlineData("a / 2")]
        [InlineData("2 / b")]
        [InlineData("2 ^ 2")]
        [InlineData("2 % 2")]
        public void InvalidFormat_ThrowsFormatException(string expression)
        {
            Action action = () => _calculator.ParseExpression(expression);
            action.Should().Throw<FormatException>();
        }
    }

    public class StringCalculatorFixture
    {
        public StringCalculator Calculator { get; } = new StringCalculator();
    }

    public class CallerCountTests : IClassFixture<StringCalculatorFixture>
    {
        private readonly StringCalculator _calculator;
        private readonly ITestOutputHelper _output;

        public CallerCountTests(StringCalculatorFixture fixture, ITestOutputHelper output)
        {
            _calculator = fixture.Calculator;
            _output = output;
        }

        [Fact]
        public void Increments_WhenAddIsCalled()
        {
            _calculator.Reset();
            _calculator.Add("1");
            _calculator.Add("2");

            _output.WriteLine($"CallerCount = {_calculator.GetCallerCount()}");

            _calculator.GetCallerCount().Should().Be(2);
        }

        [Fact]
        public void Reset_SetsCountToZero()
        {
            _calculator.Add("1");
            _calculator.Reset();

            _calculator.GetCallerCount().Should().Be(0);
        }
    }
}

