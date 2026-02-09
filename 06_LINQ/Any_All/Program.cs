using System;

namespace Coding.Exercise
{
    public class Exercise
    {
        public static bool IsAnyWordWhiteSpace(List<string> words)
        {
            //your code goes here
            return words.Any(word => word.All(char.IsWhiteSpace));
        }
    }
}
