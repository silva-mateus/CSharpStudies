using System.Diagnostics;
using IX05_AsyncAwait_ParallelDataFetcher;
using IX05_AsyncAwait_ParallelDataFetcher.DataSources;

var sources = new IDataSource[]
{
    new FastDatabaseSource(),
    new SlowApiSource(),
    new UnreliableCacheSource()
};

var fetcher = new ParallelDataFetcher(sources, maxConcurrency: 2);

// Demo 1: FetchAllAsync with timeout
Console.WriteLine("=== FetchAllAsync (3s timeout) ===");
var sw = Stopwatch.StartNew();
try
{
    var results = await fetcher.FetchAllAsync("C# interview", TimeSpan.FromSeconds(3));
    sw.Stop();
    Console.WriteLine($"Completed in {sw.ElapsedMilliseconds}ms, {results.Count} result(s):");
    foreach (var r in results)
    {
        Console.WriteLine($"  [{r.SourceName}] {r.Data} ({r.ResponseTime.TotalMilliseconds:F0}ms)");
    }
}
catch (AggregateException ex)
{
    Console.WriteLine($"All sources failed: {ex.InnerExceptions.Count} errors");
}

// Demo 2: FetchAsResultsArrive with streaming
Console.WriteLine("\n=== FetchAsResultsArrive (streaming) ===");
using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
try
{
    await foreach (var result in fetcher.FetchAsResultsArrive("async patterns", cts.Token))
    {
        Console.WriteLine($"  Received from [{result.SourceName}]: {result.Data} ({result.ResponseTime.TotalMilliseconds:F0}ms)");
    }
    Console.WriteLine("  All sources completed.");
}
catch (OperationCanceledException)
{
    Console.WriteLine("  Streaming was cancelled due to timeout.");
}
