using System;

namespace Coding.Exercise
{
    public class Exercise
    {
        public static int CountListsContainingZeroLongerThan(
             int length,
             List<List<int>> listsOfNumbers)
        {
            return listsOfNumbers.Count(list => list.Contains(0) && list.Count > length);
        }
    }
}
