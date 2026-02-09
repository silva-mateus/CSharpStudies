using System;

namespace Coding.Exercise
{
    public class Exercise
    {
        public static double CalculateAverageDurationInMilliseconds(IEnumerable<TimeSpan> timeSpans)
        {
            if (timeSpans.Count() == 0)
            {
                throw new Exception("The collection is empty");
            }
            return timeSpans.Average(timeSpan => timeSpan.TotalMilliseconds);
        }
    }
}
