# Handling exceptions with continuations
Finish the implementation of the Test method. This method calls the ParseToIntAndPrint method which may throw three kinds of exceptions:

1) ArgumentNullException, which should be handled by printing "The input is null." to the console.
2) FormatException, which should be handled by printing "The input is not in a correct format." to the console.
3) ArgumentOutOfRangeException, which should NOT be handled. If this exception is thrown within the ParseToIntAndPrint method, then "Unexpected exception type." should be printed to the console, and the task should become faulted.

If no exception is thrown, then the task should successfully run to completion.