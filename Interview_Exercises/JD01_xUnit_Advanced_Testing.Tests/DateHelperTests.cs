using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace JD01_xUnit_Advanced_Testing.Tests;

/// <summary>
/// TODO: Write comprehensive tests for DateHelper.
///
/// MINIMUM 8 TESTS for DateHelper:
/// 13. GetBusinessDaysBetween for Monday-Friday same week = 4
/// 14. GetBusinessDaysBetween spanning a weekend (Friday to next Monday = 1)
/// 15. GetBusinessDaysBetween with start > end throws ArgumentException
/// 16. [Theory+InlineData] IsLeapYear: (2000,true), (1900,false), (2024,true), (2023,false)
/// 17. IsLeapYear(0) throws ArgumentException
/// 18. [Theory] GetQuarter: Jan=1, Apr=2, Jul=3, Oct=4
/// 19. FormatRelative: "today", "yesterday", "X days ago"
/// 20. FormatRelative: "tomorrow", "in X days"
///
/// Use FluentAssertions for all DateHelper assertions.
/// </summary>
public class DateHelperTests
{
    private readonly ITestOutputHelper _output;

    public DateHelperTests(ITestOutputHelper output)
    {
        _output = output;
    }

    [Fact]
    public void GetBusinessDaysBetween_MondayAndFriday_Returns4()
    {
        var start = new DateTime(2026, 02, 16); //Monday
        var end = new DateTime(2026, 02, 20); //Friday

        var result = DateHelper.GetBusinessDaysBetween(start, end);

        _output.WriteLine($"Between {start.ToShortDateString} and {end.ToShortDateString} we have {result} days ");

        result.Should().Be(4);
    }

    [Fact]
    public void GetBusinessDaysBetween_FridayToMonday_Returns1()
    {
        var start = new DateTime(2026, 02, 20); //Friday
        var end = new DateTime(2026, 02, 23); //Monday next week

        var result = DateHelper.GetBusinessDaysBetween(start, end);

        _output.WriteLine($"Between {start.ToShortDateString} and {end.ToShortDateString} we have {result} days ");

        result.Should().Be(1);
    }

    [Fact]
    public void GetBusinessDaysBetween_StartDateGreaterThanEndDate_ThrowsArgumentException()
    {
        var start = new DateTime(2026, 02, 16);
        var end = new DateTime(2026, 02, 15);

        Action action = () => DateHelper.GetBusinessDaysBetween(start, end);
        action.Should().Throw<ArgumentException>();
    }

    [Theory]
    [InlineData(2000, true)]
    [InlineData(1900, false)]
    [InlineData(2024, true)]
    [InlineData(2023, false)]
    public void IsLeapYear_ReturnsExpectedResult(int year, bool expected)
    {
        var result = DateHelper.IsLeapYear(year);
        result.Should().Be(expected);
    }

    [Fact]
    public void IsLeapYear_YearZero_ReturnsArgumentException()
    {
        Action action = () => DateHelper.IsLeapYear(0);
        action.Should().Throw<ArgumentException>();
    }

    [Theory]
    [InlineData(1, 1)]
    [InlineData(4, 2)]
    [InlineData(7, 3)]
    [InlineData(10, 4)]
    public void GetQuarter_ForGivenMonth_ReturnCorrectQuarter(int month, int expected)
    {
        var date = new DateTime(2000, month, 1);
        var result = DateHelper.GetQuarter(date);

        _output.WriteLine($"The month {date.Month} is in the Quarter {result}");

        result.Should().Be(expected);
    }

    public static IEnumerable<object[]> FormatRelativeTestData()
    {
        yield return new object[] { new DateTime(2000, 1, 1), "today" };
        yield return new object[] { new DateTime(2000, 1, 2), "tomorrow" };
        yield return new object[] { new DateTime(1999, 12, 31), "yesterday" };
        yield return new object[] { new DateTime(1999, 12, 29), "3 days ago" };
        yield return new object[] { new DateTime(2000, 1, 3), "in 2 days" };
    }

    [Theory]
    [MemberData(nameof(FormatRelativeTestData))]
    public void FormatRelative_ReturnsExpectedRelativeFormat(DateTime date, string expected)
    {
        var now = new DateTime(2000, 1, 1);

        var result = DateHelper.FormatRelative(date, now);

        _output.WriteLine($"Compared to ({now}), {date} is/was/willbe {result}");

        result.Should().Be(expected);

    }
}
