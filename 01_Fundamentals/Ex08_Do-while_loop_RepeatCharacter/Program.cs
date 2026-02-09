using System;

namespace Coding.Exercise
{
    public class Exercise
    {
        public static string RepeatCharacter(char character, int targetLength)
        {
            string finalString = "";
            do
            {
                finalString += character;
            }
            while (finalString.Length < targetLength);
            return finalString;

        }
    }
}