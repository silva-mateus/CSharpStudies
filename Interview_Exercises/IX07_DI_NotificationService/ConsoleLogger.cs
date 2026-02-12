namespace IX07_DI_NotificationService;

public class ConsoleLogger : ILogger
{
    public void LogInfo(string message)
    {
        Console.WriteLine($"[INFO] {message}");
    }

    public void LogError(string message, Exception? exception = null)
    {
        Console.WriteLine($"[ERROR] {message}");
        if (exception is not null)
            Console.WriteLine($"  Exception: {exception.Message}");
    }
}
