using System.Diagnostics;

namespace IX05_AsyncAwait_ParallelDataFetcher.DataSources;

/// <summary>
/// Simulates an unreliable cache source.
/// Delay: 50-150ms. 30% chance of throwing TimeoutException.
/// </summary>
public class UnreliableCacheSource : IDataSource
{
    public string Name => "UnreliableCache";

    public async Task<DataResult> FetchAsync(string query, CancellationToken cancellationToken = default)
    {
        // TODO: your code goes here
        // 1. Record start time.
        // 2. Simulate delay of 50-150ms (pass cancellationToken).
        // 3. With 30% probability, throw TimeoutException("Cache timeout").
        // 4. Return DataResult with Name, data string, and elapsed time.
        var stopwatch = Stopwatch.StartNew();

        await Task.Delay(Random.Shared.Next(50, 151), cancellationToken);

        if (Random.Shared.NextDouble() < 0.3)
        {
            throw new TimeoutException("Cache timeout");
        }

        stopwatch.Stop();

        return new DataResult(Name, $"Cache result for '{query}'", stopwatch.Elapsed);
    }
}
