using IX09_Refactoring_LegacyOrderSystem.Models;
using System.Collections.Concurrent;

namespace IX09_Refactoring_LegacyOrderSystem.DataAccess;

public class InMemoryOrderRepository : IOrderRepository
{
    private readonly ConcurrentDictionary<string, ProcessedOrderResult> _cache = new();
    public ProcessedOrderResult? Get(string orderId) => _cache.TryGetValue(orderId, out var r) ? r : null;
    public void Save(string orderId, ProcessedOrderResult result) => _cache[orderId] = result;
}
