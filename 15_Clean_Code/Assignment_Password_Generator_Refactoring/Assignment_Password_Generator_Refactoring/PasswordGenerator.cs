namespace Assignment_Password_Generator_Refactoring;

public class PasswordGenerator
{
    private readonly IRandomGenerator _random;
    private const string AlphanumericCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
    private const string SpecialCharacters = "!@#$%^&*()_-+=";

    public PasswordGenerator(IRandomGenerator random)
    {
        _random = random ?? throw new ArgumentNullException(nameof(random));
    }

    public string Generate(
        int minLength, int maxLength, bool useSpecialCharacters)
    {
        ValidateLength(minLength, maxLength);

        var passwordLength = _random.Next(minLength, maxLength + 1);

        var characterPool = useSpecialCharacters
            ? AlphanumericCharacters + SpecialCharacters
            : AlphanumericCharacters;

        return new string(Enumerable
                                    .Repeat(characterPool, passwordLength)
                                    .Select(chars => chars[_random.Next(chars.Length)])
                                    .ToArray());
    }

    private static void ValidateLength(int minLength, int maxLength)
    {
        if (minLength < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(minLength),
                "minLength must be greater than 0");
        }
        if (maxLength < minLength)
        {
            throw new ArgumentOutOfRangeException(nameof(maxLength),
                $"maxLength must be greater than minLength");
        }
    }
}