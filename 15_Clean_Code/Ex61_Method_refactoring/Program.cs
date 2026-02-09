using System;

namespace Coding.Exercise
{
    public class Exercise
    {
        //you may rename the parameters of this method,
        //but do not change their order or type
        public static List<int>? ChooseBetterPath_Refactored(
            List<int> primaryPathSectionsLengths,
            List<int> secondaryPathSectionLengths,
            int maxAcceptableSectionLength)
        {
            ValidateNoNegativeLengths(primaryPathSectionsLengths);
            ValidateNoNegativeLengths(secondaryPathSectionLengths);

            bool isPrimaryAcceptable = IsAllWithinMaxLength(primaryPathSectionsLengths, maxAcceptableSectionLength);
            bool isSecondaryAcceptable = IsAllWithinMaxLength(secondaryPathSectionLengths, maxAcceptableSectionLength);

            if (!isPrimaryAcceptable && !isSecondaryAcceptable)
            {
                return null;
            }
            else if (isPrimaryAcceptable && isSecondaryAcceptable)
            {
                return primaryPathSectionsLengths.Sum() <=
                    secondaryPathSectionLengths.Sum() ?
                    primaryPathSectionsLengths :
                    secondaryPathSectionLengths;
            }

            else if (isPrimaryAcceptable)
            {
                return primaryPathSectionsLengths;
            }
            return secondaryPathSectionLengths;
        }

        private static void ValidateNoNegativeLengths(List<int> list)
        {
            if (list.Any(number => number < 0))
            {
                throw new ArgumentException(
                    "The input collections can't contain negative lengths.");
            }
        }

        private static bool IsAllWithinMaxLength(List<int> list, int maxValue)
        {
            return list.All(number => number <= maxValue);
        }


        //do not modify this method!
        public static List<int>? ChooseBetterPath(
            List<int> lengths1,
            List<int> lengths2,
            int maxLength)
        {
            foreach (var number in lengths1)
            {
                if (number < 0)
                {
                    throw new ArgumentException(
                        "The input collections can't contain negative lengths.");
                }
            }

            foreach (var number in lengths2)
            {
                if (number < 0)
                {
                    throw new ArgumentException(
                        "The input collections can't contain negative lengths.");
                }
            }

            bool isLengths1Ok = true;
            foreach (var number in lengths1)
            {
                if (number > maxLength)
                {
                    isLengths1Ok = false;
                }
            }

            bool isLengths2Ok = true;
            foreach (var number in lengths2)
            {
                if (number > maxLength)
                {
                    isLengths2Ok = false;
                }
            }

            if (!isLengths1Ok && !isLengths2Ok)
            {
                return null;
            }
            else if (isLengths1Ok && isLengths2Ok)
            {
                if (lengths1.Sum() <= lengths2.Sum())
                {
                    return lengths1;
                }
                return lengths2;
            }
            else if (isLengths1Ok)
            {
                return lengths1;
            }
            return lengths2;
        }
    }
}
