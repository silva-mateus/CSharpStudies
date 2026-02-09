using System;

namespace Coding.Exercise
{
    public static class NumericTypesDescriber
    {
        public static string Describe(object someObject)
        {
            if (someObject is int)
                return $"Int of value {someObject}";
            else if (someObject is double)
                return $"Double of value {someObject}";
            else if (someObject is decimal)
                return $"Decimal of value {someObject}";
            else
                return null;
        }
    }
}
