using System.Runtime.CompilerServices;

namespace IX05_AsyncAwait_ParallelDataFetcher;

public class ParallelDataFetcher
{
    private readonly IReadOnlyList<IDataSource> _sources;
    private readonly int _maxConcurrency;

    public ParallelDataFetcher(IEnumerable<IDataSource> sources, int maxConcurrency = 3)
    {
        _sources = sources.ToList().AsReadOnly();
        _maxConcurrency = maxConcurrency;
    }

    /// <summary>
    /// Queries all sources concurrently with a timeout.
    /// Uses SemaphoreSlim to limit concurrency to <see cref="_maxConcurrency"/>.
    /// Returns all successful results; skips failed/timed-out sources.
    /// Throws AggregateException only if ALL sources fail.
    /// </summary>
    public async Task<IReadOnlyList<DataResult>> FetchAllAsync(string query, TimeSpan timeout)
    {
        // TODO: your code goes here
        // Hints:
        // 1. Create a CancellationTokenSource with timeout.
        // 2. Create a SemaphoreSlim(_maxConcurrency).
        // 3. Launch a Task for each source that:
        //    a. Awaits semaphore.WaitAsync(ct)
        //    b. Calls source.FetchAsync(query, ct)
        //    c. Releases semaphore in finally
        // 4. Await Task.WhenAll and collect successful results.
        // 5. If all failed, throw AggregateException with all exceptions.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Starts all source fetches concurrently and yields results as they complete.
    /// Skips sources that throw exceptions (prints error to console).
    /// Respects cancellation via <paramref name="cancellationToken"/>.
    /// </summary>
    public async IAsyncEnumerable<DataResult> FetchAsResultsArrive(
        string query,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        // TODO: your code goes here
        // Hint: Use Task.WhenAny in a loop, or System.Threading.Channels.
        // Start all fetches, then repeatedly await the next completing task.
        // Remove the yield break below and replace with your implementation.
        yield break;
    }
}
