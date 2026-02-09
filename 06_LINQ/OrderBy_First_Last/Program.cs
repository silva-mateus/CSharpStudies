using System;

namespace Coding.Exercise
{
    public class Exercise
    {
        public static string FindShortestWord(List<string> words)
        {
            //your code goes here
            if (words.Count == 0)
            {
                throw new Exception("The collection is empty");
            }
            return words.OrderBy(word => word.Length).First();
        }
    }
}
