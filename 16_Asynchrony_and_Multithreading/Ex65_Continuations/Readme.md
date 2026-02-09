# Continuations

Implement the FormatSquaredNumbersFrom1To method returning a `Task<string>`.

This method should perform two steps:

1) First, it should start a task of generating squared numbers from 1 to a given value. So, for example, for N equal to 5, this task should generate a sequence of numbers: [1, 4, 9, 16, 25].

2) As a continuation of this task, a second task shall be scheduled. This second task should format the result of the first task as a string (by concatenating them with a comma). So, for example, if the first task returned the collection of [1, 4, 9, 16, 25], the continuation should format it as "1, 4, 9, 16, 25".

If N is smaller than 0, ArgumentException should be thrown.
