using System;
using System.Linq;


namespace Coding.Exercise
{
    public class Exercise
    {
        //you may rename the parameters of this method,
        //but do not change their order or type
        public static (bool, string) IsValidName_Refactored(string name)
        {
            var (isValid, message) = ValidateNameLength(name);
            if (!isValid)
            {
                return (isValid, message);
            }
            return ValidateNameCharacters(name);
        }

        //feel free to add any helper methods here

        public static class ReturnMessages
        {
            public static string TooShort = "The name is too short.";
            public static string TooLong = "The name is too long.";
            public static string StartsWithLowercase = "The name starts with a lowercase letter.";
            public static string ContainsNonLetterCharacters = "The name contains non-letter characters.";
            public static string ContainsUppercaseLettersAfterFirstCharacter = "The name contains uppercase letters after the first character.";
            public static string Valid = "The name is valid.";
        }

        public static (bool, string) ValidateNameLength(string name)
        {
            if (name.Length < 3)
            {
                return (false, ReturnMessages.TooShort);
            }
            if (name.Length > 25)
            {
                return (false, ReturnMessages.TooLong);
            }
            return (true, string.Empty);
        }

        public static (bool, string) ValidateNameCharacters(string name)
        {
            if (name[0] == char.ToLower(name[0]))
            {
                return (false, ReturnMessages.StartsWithLowercase);
            }
            if (name.Any(c => !char.IsLetter(c)))
            {
                return (false, ReturnMessages.ContainsNonLetterCharacters);
            }
            for (int i = 1; i < name.Length; i++)
            {
                char c = name[i];
                if (c == char.ToUpper(c))
                {
                    return (false, ReturnMessages.ContainsUppercaseLettersAfterFirstCharacter);
                }
            }
            return (true, ReturnMessages.Valid);
        }



        //do not modify this method        
        public static (bool, string) IsValidName(string name)
        {
            if (name.Length < 3)
            {
                return (false, "The name is too short.");
            }
            if (IsLong(name))
            {
                return (false, "The name is too long.");
            }
            if (name[0] == char.ToLower(name[0]))
            {
                return (false, "The name starts with a lowercase letter.");
            }
            foreach (var c in name)
            {
                if (!char.IsLetter(c))
                {
                    return (false, "The name contains non-letter characters.");
                }
            }
            for (int i = 1; i < name.Length; i++)
            {
                char c = name[i];
                if (c == char.ToUpper(c))
                {
                    return (false, "The name contains uppercase letters after the first character.");
                }
            }
            return (true, "The name is valid.");
        }

        private static bool IsLong(string name)
        {
            return name.Length > 25;
        }
    }
}
