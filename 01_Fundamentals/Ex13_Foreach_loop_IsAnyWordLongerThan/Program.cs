using System;

namespace Coding.Exercise
{
    public class Exercise
    {
        public static bool IsAnyWordLongerThan(int length, string[] words)
        {
            foreach (var word in words)
            {
                if (word.Length > length)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
