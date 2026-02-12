namespace IX08_Collections_PriorityTaskScheduler;

public enum TaskPriority
{
    Critical = 0,
    High = 1,
    Normal = 2,
    Low = 3
}

public interface IScheduledTask
{
    string Id { get; }
    TaskPriority Priority { get; }
    DateTime DueDate { get; }
    void Execute();
}

/// <summary>
/// A simple implementation of IScheduledTask for testing and demo purposes.
/// Records whether Execute was called.
/// </summary>
public class SimpleTask : IScheduledTask
{
    public string Id { get; }
    public TaskPriority Priority { get; }
    public DateTime DueDate { get; }
    public bool WasExecuted { get; private set; }

    public SimpleTask(string id, TaskPriority priority, DateTime dueDate)
    {
        Id = id;
        Priority = priority;
        DueDate = dueDate;
    }

    public void Execute()
    {
        WasExecuted = true;
        Console.WriteLine($"  Executed task '{Id}' (Priority: {Priority}, Due: {DueDate:yyyy-MM-dd})");
    }
}
