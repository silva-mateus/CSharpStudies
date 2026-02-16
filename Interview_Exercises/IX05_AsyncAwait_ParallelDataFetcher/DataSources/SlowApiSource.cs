using System.Diagnostics;

namespace IX05_AsyncAwait_ParallelDataFetcher.DataSources;

/// <summary>
/// Simulates a slow REST API source.
/// Delay: 500-2000ms. 20% chance of throwing HttpRequestException.
/// </summary>
public class SlowApiSource : IDataSource
{
    public string Name => "SlowApi";

    public async Task<DataResult> FetchAsync(string query, CancellationToken cancellationToken = default)
    {
        // TODO: your code goes here
        // 1. Record start time.
        // 2. Simulate delay of 500-2000ms (pass cancellationToken).
        // 3. With 20% probability, throw HttpRequestException("API service unavailable").
        // 4. Return DataResult with Name, data string, and elapsed time.
        var stopwatch = Stopwatch.StartNew();

        await Task.Delay(Random.Shared.Next(500, 2001), cancellationToken);

        if (Random.Shared.NextDouble() < 0.2)
        {
            throw new HttpRequestException("API service unavailable");
        }

        stopwatch.Stop();

        return new DataResult(Name, $"API result for '{query}'", stopwatch.Elapsed);
    }
}
