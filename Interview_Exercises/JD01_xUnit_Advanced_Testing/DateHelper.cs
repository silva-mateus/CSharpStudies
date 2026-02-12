namespace JD01_xUnit_Advanced_Testing;

/// <summary>
/// Date utility methods. DO NOT MODIFY THIS CLASS -- write tests for it.
/// </summary>
public static class DateHelper
{
    /// <summary>
    /// Counts business days (weekdays) between start and end (exclusive of end).
    /// Throws ArgumentException if start > end.
    /// </summary>
    public static int GetBusinessDaysBetween(DateTime start, DateTime end)
    {
        if (start > end)
            throw new ArgumentException("Start date must be before or equal to end date.");

        var count = 0;
        var current = start.Date;
        var endDate = end.Date;

        while (current < endDate)
        {
            if (current.DayOfWeek != DayOfWeek.Saturday && current.DayOfWeek != DayOfWeek.Sunday)
                count++;
            current = current.AddDays(1);
        }

        return count;
    }

    /// <summary>
    /// Returns true if the given year is a leap year.
    /// Throws ArgumentException if year is less than or equal to 0.
    /// </summary>
    public static bool IsLeapYear(int year)
    {
        if (year <= 0)
            throw new ArgumentException("Year must be positive.", nameof(year));

        return (year % 4 == 0 && year % 100 != 0) || (year % 400 == 0);
    }

    /// <summary>
    /// Returns the quarter (1-4) for the given date.
    /// </summary>
    public static int GetQuarter(DateTime date)
    {
        return (date.Month - 1) / 3 + 1;
    }

    /// <summary>
    /// Returns a human-readable relative date string.
    /// Examples: "today", "yesterday", "3 days ago", "tomorrow", "in 5 days"
    /// </summary>
    public static string FormatRelative(DateTime date, DateTime now)
    {
        var diff = (date.Date - now.Date).Days;

        return diff switch
        {
            0 => "today",
            -1 => "yesterday",
            1 => "tomorrow",
            < 0 => $"{Math.Abs(diff)} days ago",
            > 0 => $"in {diff} days"
        };
    }
}
