using System;

namespace Coding.Exercise
{
    public class Exercise
    {
        public static IEnumerable<DateTime> GetFridaysOfYear(int year, IEnumerable<DateTime> dates)
        {
            return dates.Where(date => date.Year == year && date.DayOfWeek == DayOfWeek.Friday).Distinct();
        }
    }
}
