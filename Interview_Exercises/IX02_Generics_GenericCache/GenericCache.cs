namespace IX02_Generics_GenericCache;

public class GenericCache<TKey, TValue> where TKey : notnull
{
    private readonly ITimeProvider _timeProvider;
    private readonly Dictionary<TKey, (TValue value, DateTime expiryTime)> _cache;  

    public GenericCache(ITimeProvider? timeProvider = null)
    {
        _timeProvider = timeProvider ?? new SystemTimeProvider();
        _cache = new();
    }

    /// <summary>
    /// Adds an item to the cache with the given expiration duration.
    /// Throws <see cref="ArgumentException"/> if the key already exists.
    /// </summary>
    public void Add(TKey key, TValue value, TimeSpan expiry)
    {
        if (_cache.ContainsKey(key))
            throw new ArgumentException($"Key '{key}' already exists.");

        _cache[key] = (value, _timeProvider.UtcNow.Add(expiry));
    }

    /// <summary>
    /// Attempts to retrieve a cached value. Returns false if the key
    /// does not exist or has expired.
    /// </summary>
    public bool TryGet(TKey key, out TValue? value)
    {
        if (_cache.TryGetValue(key, out var entry))
        {
            if (_timeProvider.UtcNow < entry.expiryTime)
            {
                value = entry.value;
                return true;
            }

            _cache.Remove(key);
        }

        value = default;
        return false;
    }

    /// <summary>
    /// Removes an entry from the cache.
    /// Returns true if the entry existed, false otherwise.
    /// </summary>
    public bool Remove(TKey key) => _cache.Remove(key);

    /// <summary>
    /// Returns the count of non-expired entries.
    /// </summary>
    public int Count => _cache.Count(x => _timeProvider.UtcNow < x.Value.expiryTime);

    /// <summary>
    /// Removes all expired entries from internal storage.
    /// </summary>
    public void Cleanup()
    {
        var now = _timeProvider.UtcNow;
        var expiredKeys = _cache
            .Where(x => now >= x.Value.expiryTime)
            .Select(x => x.Key)
            .ToList();

        foreach (var key in expiredKeys)
            _cache.Remove(key);
    }
}
