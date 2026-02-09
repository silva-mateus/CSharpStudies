using System;

namespace Coding.Exercise
{
    public class Exercise
    {
        public static int CalculateSumOfNumbersBetween(int firstNumber, int lastNumber)
        {
            int sum = 0;
            int current = firstNumber;
            while (current <= lastNumber)
            {
                sum += current;
                current += 1;

            }
            return sum;
        }
    }
}
