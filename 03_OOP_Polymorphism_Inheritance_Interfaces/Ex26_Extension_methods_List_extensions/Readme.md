Extension methods - List extensions

Create an extension method for the List<int> type called TakeEverySecond.

This method should return a new List of ints with every second element from the input list.

For example:

for input  { 1, 5, 10, 8, 12, 4, 5 }, the result shall be { 1, 10, 12, 5 }

for input  { 1, 5, 10, 8, 12, 4, 5, 6 }, the result shall be { 1, 10, 12, 5 }

for input  { 1 } , the result shall be { 1 }

for empty input, the result shall be empty

don't handle the null input in any way (let it throw an exception)

It must be possible to call this method like this:



