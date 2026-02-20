namespace IX09_Refactoring_LegacyOrderSystem.Logger;

public class ConsoleLogger : ILogger
{
    public void LogError(string message, Exception? exception = null)
    {
        Console.WriteLine($"[ERROR]: {message}");
        if (exception != null)
            Console.WriteLine($"Exception: {exception.Message}");
    }

    public void LogInfo(string message)
    {
        Console.WriteLine($"[INFO]: {message}");
    }
}

