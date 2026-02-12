namespace IX05_AsyncAwait_ParallelDataFetcher;

public record DataResult(string SourceName, string Data, TimeSpan ResponseTime);

public interface IDataSource
{
    string Name { get; }
    Task<DataResult> FetchAsync(string query, CancellationToken cancellationToken = default);
}
