namespace IX04_ErrorHandling_RetryPolicy;

/// <summary>
/// Abstraction for delays and time, enabling testability without real waits.
/// </summary>
public interface IDelayProvider
{
    Task DelayAsync(TimeSpan delay);
    DateTime UtcNow { get; }
}

/// <summary>
/// Real implementation that uses Task.Delay and DateTime.UtcNow.
/// </summary>
public class SystemDelayProvider : IDelayProvider
{
    public Task DelayAsync(TimeSpan delay) => Task.Delay(delay);
    public DateTime UtcNow => DateTime.UtcNow;
}


