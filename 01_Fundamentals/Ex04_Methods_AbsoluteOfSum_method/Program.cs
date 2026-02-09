using System;

namespace Coding.Exercise
{
    public class Exercise
    {
        public static int AbsoluteOfSum(int a, int b)
        {
            int sum = a + b;
            if (sum < 0)
            {
                sum = -sum;
            }
            return sum;
        }
    }
}
