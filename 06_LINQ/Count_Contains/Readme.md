# Count & Contains
Implement the CountListsContainingZeroLongerThan method. It takes the following parameters:

- length of type int
- listsOfNumbers of type List of Lists of ints

This method should return the count of the lists that meet the following conditions:

1) Contain the number zero
2) Are longer than length

For example, for length 3 and the following lists contained in the listsOfNumbers:

- {1, 2, 5, -1}
- {0, 4, 4, 6}
- {9, 0}

The result shall be 1 because there is only one list that contains zero, and has more than 3 items.