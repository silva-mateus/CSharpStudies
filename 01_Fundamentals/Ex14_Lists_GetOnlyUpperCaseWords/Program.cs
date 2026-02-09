using System;

namespace Coding.Exercise
{
    public class Exercise
    {
        public List<string> GetOnlyUpperCaseWords(List<string> words)
        {
            var finalList = new List<string>();
            var iNonUpperFlag = false;
            foreach (var word in words)
            {
                foreach (char character in word)
                {
                    if (!char.IsUpper(character))
                    {
                        iNonUpperFlag = true;
                        break;
                    }
                }
                if (iNonUpperFlag == false && !finalList.Contains(word))
                {
                    finalList.Add(word);
                }
                else
                {
                    iNonUpperFlag = false;
                }

            }
            return finalList;
        }
    }
}
