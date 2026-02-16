using System.Diagnostics;

namespace IX05_AsyncAwait_ParallelDataFetcher.DataSources;

/// <summary>
/// Simulates a fast database source.
/// Delay: 100-300ms. Always succeeds.
/// </summary>
public class FastDatabaseSource : IDataSource
{
    public string Name => "FastDatabase";

    public async Task<DataResult> FetchAsync(string query, CancellationToken cancellationToken = default)
    {
        // TODO: your code goes here
        // 1. Record start time with Stopwatch or DateTime.
        // 2. Simulate delay of 100-300ms using Task.Delay (pass cancellationToken).
        // 3. Return DataResult with Name, data string, and elapsed time.

        var stopwatch = Stopwatch.StartNew();        

        await Task.Delay(Random.Shared.Next(100, 301), cancellationToken);

        stopwatch.Stop();

        return new DataResult(Name, $"DB result for '{query}'", stopwatch.Elapsed);

    }
}
