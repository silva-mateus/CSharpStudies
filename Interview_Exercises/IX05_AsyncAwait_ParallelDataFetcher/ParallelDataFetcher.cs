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
        using var cts = new CancellationTokenSource(timeout);
        var ct = cts.Token;

        using var semaphore = new SemaphoreSlim(_maxConcurrency);

        var successes = new List<DataResult>();
        var errors = new List<Exception>();
        var sync = new object();


        var tasks = _sources.Select(async source =>
        {
            try
            {
                await semaphore.WaitAsync(ct);
                try
                {
                    var result = await source.FetchAsync(query, ct);
                    lock (sync)
                    {
                        successes.Add(result);
                    }
                }
                finally
                {
                    semaphore.Release();
                }
            }
            catch (OperationCanceledException ex)
            {
                lock (sync)
                {
                    errors.Add(ex);
                }
            }
            catch (Exception ex)
            {
                lock (sync)
                {
                    errors.Add(ex);
                }
            }
        });

        await Task.WhenAll(tasks);

        if (successes.Count > 0)
            return successes;

        throw new AggregateException("All sources failed.", errors);
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

        DataResult result;

        var pending = _sources.ToDictionary(
            source => source.FetchAsync(query, cancellationToken),
            source => source);

        while (pending.Count > 0)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var completedTask = await Task.WhenAny(pending.Keys);
            var source = pending[completedTask];

            pending.Remove(completedTask);

            try
            {
                result = await completedTask;
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[{source.Name}] failed: {ex.Message}");
                continue;
            }

            yield return result;
        }
    }
}
