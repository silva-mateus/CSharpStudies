using System;

namespace Coding.Exercise
{
    public static class ExceptionsRethrowing
    {
        public static int GetMaxValue(List<int> numbers)
        {
            try
            {
                return numbers.Max();
            }
            catch (ArgumentNullException ex)
            {
                throw new ArgumentNullException("The numbers list cannot be null.", ex);
            }
            catch (InvalidOperationException)
            {
                Console.WriteLine("The numbers list cannot be empty.");
                throw;
            }
        }
    }
}
