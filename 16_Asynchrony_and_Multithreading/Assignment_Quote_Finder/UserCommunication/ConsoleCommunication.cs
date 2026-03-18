namespace Assignment_Quote_Finder.UserCommunication;

internal class ConsoleCommunication : IUserCommunication
{
    public void PrintMessage(string message) => Console.WriteLine(message); 
    public bool ReadBool(string prompt) => ReadWithValidation(prompt, TryParseBool);
    public int ReadInt(string prompt) => ReadWithValidation(prompt, TryParseInt);
    public string ReadWord(string prompt) => ReadWithValidation(prompt, TryParseWord);

    private T ReadWithValidation<T>(string prompt, Func<string?, (bool success, T? value, string? error)> parser)
    {
        do
        {
            try
            {
                PrintMessage(prompt);
                var input = Console.ReadLine();
                var (success, value, error) = parser(input);

                if (success)
                    return value!;

                if (error != null)
                    PrintMessage("Error: " + error);
            }
            catch (Exception ex)
            {
                PrintMessage("Exception: " + ex.Message);
            }
        }
        while (true);
    }

    private (bool success, bool value, string? error) TryParseBool(string? input)
    {
        if (input == "y")
            return (true, true, null);
        if (input == "n")
            return (true, false, null);

        return (false, false, "Type 'y' or 'n'");
    }
    private (bool success, int value, string? error) TryParseInt(string? input)
    {
        if (int.TryParse(input, out int result))
        {
            if (result > 0)
            {
                return (true, result, null);
            }
        }

        return (false, 0, "Invalid Value. Type a integer number");
    }
    private (bool success, string value, string? error) TryParseWord(string? input)
    {
        if (input == null)
            return (false, string.Empty, "Input can't be empty");
        if (!input.All(char.IsLetter))
            return (false, string.Empty, "Type only letters");

        return (true, input.ToLower(), null);
    }
}

