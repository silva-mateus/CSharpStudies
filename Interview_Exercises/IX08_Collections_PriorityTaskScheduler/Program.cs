using IX08_Collections_PriorityTaskScheduler;

var scheduler = new PriorityTaskScheduler<SimpleTask>();

// Add tasks with various priorities and dates
scheduler.Enqueue(new SimpleTask("task-1", TaskPriority.Low, new DateTime(2025, 6, 1)));
scheduler.Enqueue(new SimpleTask("task-2", TaskPriority.Critical, new DateTime(2025, 3, 15)));
scheduler.Enqueue(new SimpleTask("task-3", TaskPriority.Normal, new DateTime(2025, 1, 10)));
scheduler.Enqueue(new SimpleTask("task-4", TaskPriority.High, new DateTime(2025, 2, 20)));
scheduler.Enqueue(new SimpleTask("task-5", TaskPriority.Critical, new DateTime(2025, 1, 5)));
scheduler.Enqueue(new SimpleTask("task-6", TaskPriority.Normal, new DateTime(2025, 4, 1)));

Console.WriteLine($"Scheduler has {scheduler.Count} tasks\n");

// Iterate without modifying (uses IEnumerable)
Console.WriteLine("=== Tasks in priority order (iteration, non-destructive) ===");
foreach (var task in scheduler)
{
    Console.WriteLine($"  {task.Id}: {task.Priority}, Due: {task.DueDate:yyyy-MM-dd}");
}

Console.WriteLine($"\nScheduler still has {scheduler.Count} tasks after iteration\n");

// Execute one by one
Console.WriteLine("=== ExecuteNext (highest priority) ===");
scheduler.ExecuteNext();

Console.WriteLine($"\nRemaining: {scheduler.Count} tasks\n");

// Execute all remaining with parallelism of 2
Console.WriteLine("=== ExecuteAllAsync (maxParallelism=2) ===");
await scheduler.ExecuteAllAsync(maxParallelism: 2);

Console.WriteLine($"\nRemaining: {scheduler.Count} tasks");
