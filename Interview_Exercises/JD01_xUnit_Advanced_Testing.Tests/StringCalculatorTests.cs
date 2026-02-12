using JD01_xUnit_Advanced_Testing;
using Xunit;
using Xunit.Abstractions;
using FluentAssertions;

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
}
