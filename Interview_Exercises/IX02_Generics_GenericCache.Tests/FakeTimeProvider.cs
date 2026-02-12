using IX02_Generics_GenericCache;

namespace IX02_Generics_GenericCache.Tests;

/// <summary>
/// A fake time provider that lets tests control the current time.
/// Use <see cref="Advance"/> to simulate time passing.
/// </summary>
public class FakeTimeProvider : ITimeProvider
{
    public DateTime UtcNow { get; private set; } = new(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    public void Advance(TimeSpan duration)
    {
        UtcNow = UtcNow.Add(duration);
    }

    public void SetTime(DateTime time)
    {
        UtcNow = time;
    }
}
