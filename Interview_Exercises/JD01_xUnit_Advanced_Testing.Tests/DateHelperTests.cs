using JD01_xUnit_Advanced_Testing;
using Xunit;
using Xunit.Abstractions;
using FluentAssertions;

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
    // TODO: your tests go here
}
