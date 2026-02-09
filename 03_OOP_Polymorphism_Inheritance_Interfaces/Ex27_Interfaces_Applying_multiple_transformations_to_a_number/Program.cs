using System;

namespace Coding.Exercise
{
    public static class Exercise
    {
        public static int Transform(
            int number)
        {
            var transformations = new List<INumericTransformation>
            {
                new By1Incrementer(),
                new By2Multiplier(),
                new ToPowerOf2Raiser()
            };

            var result = number;
            foreach (var transformation in transformations)
            {
                result = transformation.Transform(result);
            }
            return result;
        }
    }

    public interface INumericTransformation
    {
        int Transform(int number);
    }

    public class By1Incrementer : INumericTransformation
    {
        public int Transform(int number)
        {
            return number + 1;
        }
    }

    public class By2Multiplier : INumericTransformation
    {
        public int Transform(int number)
        {
            return number * 2;
        }
    }
    
    public class ToPowerOf2Raiser : INumericTransformation
    {
        public int Transform(int number)
        {
            return (int)Math.Pow(number, 2);
        }
    }
}
