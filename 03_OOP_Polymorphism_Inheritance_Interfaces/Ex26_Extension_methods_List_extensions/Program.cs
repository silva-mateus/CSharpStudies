using System;

namespace Coding.Exercise
{
    public static class ListExtensions
    {
        public static List<int> TakeEverySecond(this List<int> list)
        {
            var result = new List<int>();
            for (int i = 0; i < list.Count; i += 2)
            {
                result.Add(list[i]);
            }
            return result;
        }
    }
}
