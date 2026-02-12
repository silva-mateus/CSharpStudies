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
        throw new NotImplementedException();
    }
}
