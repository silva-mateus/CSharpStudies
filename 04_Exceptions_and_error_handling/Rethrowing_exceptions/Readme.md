# Rethrowing exceptions
In the exercise, you will find the GetMaxValue method, which tries to find a max value in a collection of ints. Currently, this method throws the following exceptions:

- ArgumentNullException when the input List is null
- InvalidOperationException when the input List is empty

Modify this method in such a way that:

- When the List is null, a new ArgumentNullException will be thrown, with the message set to "The numbers list cannot be null." Its InnerException property should be set to the original ArgumentNullException thrown by the Max method.

- When the List is empty, the "The numbers list cannot be empty." message should be printed to the console. Then, the original exception thrown by the Max method should be rethrown.