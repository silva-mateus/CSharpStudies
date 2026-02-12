using IX04_ErrorHandling_RetryPolicy;

var delayProvider = new SystemDelayProvider();

// Demo: Retry Policy
Console.WriteLine("=== Retry Policy Demo ===");
var retryPolicy = new RetryPolicy(
    maxRetries: 3,
    initialDelay: TimeSpan.FromSeconds(1),
    delayProvider: delayProvider,
    typeof(HttpRequestException));

var attemptCount = 0;
try
{
    var result = await retryPolicy.ExecuteAsync<string>(async () =>
    {
        attemptCount++;
        Console.WriteLine($"  Attempt #{attemptCount}...");
        if (attemptCount < 3)
            throw new HttpRequestException("Service unavailable");
        return "Success!";
    });
    Console.WriteLine($"  Result: {result}");
}
catch (Exception ex)
{
    Console.WriteLine($"  Failed after retries: {ex.Message}");
}

// Demo: Circuit Breaker
Console.WriteLine("\n=== Circuit Breaker Demo ===");
var breaker = new CircuitBreaker(
    failureThreshold: 3,
    openDuration: TimeSpan.FromSeconds(5),
    delayProvider: delayProvider);

for (int i = 0; i < 10; i++)
{
    try
    {
        var result = await breaker.ExecuteAsync<string>(async () =>
        {
            await Task.Delay(1000);
            throw new Exception("Service error");
        });
    }
    catch (CircuitBreakerOpenException)
    {
        Console.WriteLine($"  Call {i + 1}: Circuit is OPEN - call rejected");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"  Call {i + 1}: Failed - {ex.Message} (State: {breaker.State})");
    }
    await Task.Delay(1000); // help check the transition between Closed -> Open -> HalfOpen -> Open
}
