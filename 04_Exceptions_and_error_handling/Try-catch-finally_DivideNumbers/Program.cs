using System;

namespace Coding.Exercise
{
    public static class ExceptionsDivisionExercise
    {
        public static int DivideNumbers(int a, int b)
        {
            //your code goes here
            try
            {
                return a / b;
            }
            catch (DivideByZeroException)
            {
                Console.WriteLine("Division by zero.");
                return 0;
            }
            finally
            {
                Console.WriteLine("The DivideNumbers method ends.");
            }

            //your code goes here
        }
    }
}
