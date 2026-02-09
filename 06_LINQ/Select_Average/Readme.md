# Select & Average
TimeSpan is a type representing a time interval. We can use it to, for example, represent how much time some operation took.

It has properties like TotalHours, TotalMinutes, or TotalMilliseconds.

Implement the CalculateAverageDurationInMilliseconds method, which takes a collection of timeSpans and returns their average duration in milliseconds.

For example, for timespans of length 100, 50, and 30 milliseconds, the result shall be 60 (because the average is (100 + 50 + 30) / 3, which gives 60).

For an empty collection, the method should throw an exception.