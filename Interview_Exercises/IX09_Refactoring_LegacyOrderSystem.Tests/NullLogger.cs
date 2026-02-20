using IX09_Refactoring_LegacyOrderSystem.Logger;

namespace IX09_Refactoring_LegacyOrderSystem.Tests;

public class NullLogger : ILogger
{
    public void LogError(string message, Exception? exception = null)
    {
    }

    public void LogInfo(string message)
    {
    }
}
