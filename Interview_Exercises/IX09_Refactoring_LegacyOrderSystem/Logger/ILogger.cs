namespace IX09_Refactoring_LegacyOrderSystem.Logger;

public interface ILogger
{
    void LogInfo(string message);
    void LogError(string message, Exception? exception = null);
}

