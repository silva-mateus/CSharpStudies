using IX04_ErrorHandling_RetryPolicy;

namespace IX04_ErrorHandling_RetryPolicy.Tests;

/// <summary>
/// A fake delay provider that records delays without actually waiting,
/// and allows tests to control the current time.
/// </summary>
public class FakeDelayProvider : IDelayProvider
{
    private DateTime _utcNow = new(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    public List<TimeSpan> RecordedDelays { get; } = new();

    public DateTime UtcNow => _utcNow;

    public Task DelayAsync(TimeSpan delay)
    {
        RecordedDelays.Add(delay);
        _utcNow = _utcNow.Add(delay);
        return Task.CompletedTask;
    }

    public void Advance(TimeSpan duration)
    {
        _utcNow = _utcNow.Add(duration);
    }
}
