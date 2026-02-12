using System.Collections;

namespace IX08_Collections_PriorityTaskScheduler;

/// <summary>
/// A generic binary min-heap. Do NOT use System.Collections.Generic.PriorityQueue.
/// Implement this from scratch using an array-based binary heap.
///
/// The heap uses an IComparer&lt;T&gt; to determine ordering.
/// The smallest element (per the comparer) is always at the root.
/// </summary>
public class MinHeap<T> : IEnumerable<T>
{
    private readonly IComparer<T> _comparer;

    // TODO: Define internal storage (e.g., List<T> for the array-based heap)

    public MinHeap(IComparer<T>? comparer = null)
    {
        _comparer = comparer ?? Comparer<T>.Default;
    }

    public int Count
    {
        // TODO: your code goes here
        get => throw new NotImplementedException();
    }

    public bool IsEmpty => Count == 0;

    /// <summary>
    /// Adds an item to the heap and restores the heap property (bubble up).
    /// Time complexity: O(log n).
    /// </summary>
    public void Enqueue(T item)
    {
        // TODO: your code goes here
        // 1. Add item to end of array.
        // 2. Bubble up: while item < parent, swap with parent.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Removes and returns the minimum item (root). Restores heap property (bubble down).
    /// Time complexity: O(log n).
    /// Throws InvalidOperationException if empty.
    /// </summary>
    public T Dequeue()
    {
        // TODO: your code goes here
        // 1. Save root item.
        // 2. Move last item to root.
        // 3. Bubble down: while item > smallest child, swap with smallest child.
        // 4. Return saved root.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Returns the minimum item without removing it.
    /// Throws InvalidOperationException if empty.
    /// </summary>
    public T Peek()
    {
        // TODO: your code goes here
        throw new NotImplementedException();
    }

    /// <summary>
    /// Creates a shallow copy of the heap for safe enumeration.
    /// </summary>
    public MinHeap<T> Clone()
    {
        // TODO: your code goes here
        // Create a new MinHeap and copy internal storage.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Enumerates items in priority order by dequeuing from a cloned heap.
    /// Does NOT modify the original heap.
    /// </summary>
    public IEnumerator<T> GetEnumerator()
    {
        // TODO: your code goes here
        // Clone the heap, then dequeue all items from the clone.
        throw new NotImplementedException();
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
