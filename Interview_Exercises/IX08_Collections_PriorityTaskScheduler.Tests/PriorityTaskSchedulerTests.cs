using IX08_Collections_PriorityTaskScheduler;

namespace IX08_Collections_PriorityTaskScheduler.Tests;

/// <summary>
/// TODO: Write unit tests for MinHeap and PriorityTaskScheduler.
///
/// MinHeap tests:
///  1. Enqueue items and Dequeue returns them in sorted order.
///  2. Peek returns the min without removing.
///  3. Dequeue on empty heap throws InvalidOperationException.
///  4. IEnumerable iterates in sorted order without modifying the heap.
///
/// PriorityTaskScheduler tests:
///  5. Tasks dequeue in priority order (Critical first).
///  6. Same priority ordered by DueDate (earlier first).
///  7. Peek returns highest-priority task without removing.
///  8. Empty scheduler: Dequeue/Peek throw InvalidOperationException.
///  9. Duplicate task Id throws ArgumentException.
/// 10. Contains returns true for existing, false for missing.
/// 11. ExecuteNext: Dequeues and calls Execute on highest-priority task.
/// 12. ExecuteAllAsync: All tasks are executed (verify WasExecuted on SimpleTask).
/// 13. IEnumerable: Iteration returns sorted items; Count unchanged after iteration.
/// 14. Thread safety: Concurrent Enqueue from multiple Tasks doesn't corrupt state.
///     (Launch 100 Tasks that each Enqueue a unique SimpleTask, then verify Count = 100.)
/// </summary>
public class MinHeapTests
{
    // TODO: your tests go here
}

public class PriorityTaskSchedulerTests
{
    // TODO: your tests go here
}
