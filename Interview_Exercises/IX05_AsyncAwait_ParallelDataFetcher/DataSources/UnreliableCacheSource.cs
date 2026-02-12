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
        throw new NotImplementedException();
    }
}
