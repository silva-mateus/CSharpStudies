# IX08 - Collections: Priority Task Scheduler

## Difficulty: Hard
## Estimated Time: 90-120 minutes
## Type: Create from scratch (with unit tests)

## Overview

Implement a priority-based task scheduler backed by a custom min-heap data structure. Tasks are scheduled by priority level and due date. The scheduler must be thread-safe and support both sequential and parallel execution. This exercise tests deep knowledge of data structures, generics, thread safety, and `IEnumerable` implementation.

## Requirements

### Data Model (provided)

```csharp
public enum TaskPriority { Critical = 0, High = 1, Normal = 2, Low = 3 }

public interface IScheduledTask
{
    string Id { get; }
    TaskPriority Priority { get; }
    DateTime DueDate { get; }
    void Execute();
}
```

### Custom Min-Heap: `PriorityQueue<T>` (internal)

Implement a binary min-heap that sorts by a comparison function. This is the internal backbone of the scheduler.

**Do NOT use `System.Collections.Generic.PriorityQueue`** -- implement your own.

#### Operations

| Operation | Time Complexity | Description |
|-----------|----------------|-------------|
| `Enqueue(T item)` | O(log n) | Add item and bubble up |
| `Dequeue()` | O(log n) | Remove and return min item, bubble down |
| `Peek()` | O(1) | Return min item without removing |
| `Count` | O(1) | Number of items |
| `IsEmpty` | O(1) | Whether queue is empty |

### PriorityTaskScheduler<T> where T : IScheduledTask

#### Constructor
- Accepts an optional `IComparer<T>` (default: compare by Priority ascending, then DueDate ascending).

#### Methods

| Method | Description |
|--------|-------------|
| `void Enqueue(T task)` | Thread-safe enqueue. Throws if task with same Id already exists. |
| `T Dequeue()` | Thread-safe dequeue. Throws `InvalidOperationException` if empty. |
| `T Peek()` | Thread-safe peek. Throws `InvalidOperationException` if empty. |
| `int Count` | Thread-safe count. |
| `bool Contains(string taskId)` | Check if a task with given Id is in the scheduler. |
| `void ExecuteNext()` | Dequeue the highest-priority task and call `Execute()`. |
| `Task ExecuteAllAsync(int maxParallelism = 1)` | Dequeue and execute all tasks, up to `maxParallelism` concurrently. Use `SemaphoreSlim`. |

#### Thread Safety
- Use `ReaderWriterLockSlim`:
  - Read lock for: `Peek`, `Count`, `Contains`
  - Write lock for: `Enqueue`, `Dequeue`, `ExecuteNext`
  - `ExecuteAllAsync` should hold the write lock only during dequeue, not during execution.

#### IEnumerable<T>
- Implement `IEnumerable<T>` that iterates items in priority order (by dequeuing from a copy of the heap).
- The original heap must NOT be modified during enumeration.

## Unit Tests to Write

Create tests covering:

1. **Enqueue/Dequeue**: Items come out in priority order (Critical before High before Normal).
2. **Same priority**: Items with same priority are ordered by DueDate (earlier first).
3. **Peek**: Returns highest-priority item without removing it.
4. **Empty scheduler**: Dequeue and Peek throw `InvalidOperationException`.
5. **Duplicate Id**: Enqueue throws `ArgumentException`.
6. **Contains**: Returns true/false correctly.
7. **ExecuteNext**: Executes highest-priority task (verify via a tracking mechanism).
8. **ExecuteAllAsync**: All tasks are executed (verify via tracking).
9. **IEnumerable**: Iteration returns items in priority order without modifying the scheduler.
10. **Thread safety**: Concurrent Enqueue/Dequeue from multiple tasks does not corrupt state.

## Constraints

- Do NOT use `System.Collections.Generic.PriorityQueue<TElement, TPriority>`.
- Implement a real binary min-heap (array-based).
- Use `ReaderWriterLockSlim` (not `lock`) for thread safety.
- The heap must be generic and reusable.

## Topics Covered

- Custom data structures (binary heap)
- Generics with constraints
- `IEnumerable<T>` implementation
- Thread safety with `ReaderWriterLockSlim`
- `SemaphoreSlim` for parallelism control
- Algorithm complexity (O(log n) insert/remove)
- Async execution patterns
