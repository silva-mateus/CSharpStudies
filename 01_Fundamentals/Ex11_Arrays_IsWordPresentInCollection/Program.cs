using System;

namespace Coding.Exercise
{
    public class Exercise
    {
        public static bool IsWordPresentInCollection(string[] words, string wordToBeChecked)
        {
            for (int i = 0; i < words.Length; ++i)
            {
                if (words[i] == wordToBeChecked)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
