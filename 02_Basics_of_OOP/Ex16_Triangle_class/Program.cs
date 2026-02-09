using System;

namespace Coding.Exercise
{
    public class Triangle
    {
        private int Base;
        private int Height;

        public Triangle(int @base, int height)
        {
            Base = @base;
            Height = height;
        }

        public decimal CalculateArea()
        {
            return ((Base * Height) / 2);
        }

        public string AsString()
        {
            return $"Base is {Base}, height is {Height}";
        }
    }
}
