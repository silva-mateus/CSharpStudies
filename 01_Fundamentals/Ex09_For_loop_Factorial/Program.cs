using System;

namespace Coding.Exercise
{
    public class Exercise
    {
        public static int Factorial(int number)
        {
            var result = 1;
            if (number > 0)
            {
                for (int i = 1; i <= number; ++i)
                {
                    result = result * i;
                }
            }
            return result;
        }
    }
}
