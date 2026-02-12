namespace UserManagement;

// ISSUE: static utility class with mutable state - anti-pattern
public static class CacheHelper
{
    private static Dictionary<string, object> _globalCache = new(); // ISSUE: not thread-safe
    private static Dictionary<string, DateTime> _cacheTimestamps = new();

    public static void Set(string key, object value)
    {
        _globalCache[key] = value;
        _cacheTimestamps[key] = DateTime.Now; // ISSUE: DateTime.Now instead of UtcNow
    }

    public static object? Get(string key)
    {
        if (!_globalCache.ContainsKey(key))
            return null;

        // ISSUE: expiration check is wrong - items expire after 1 second instead of reasonable duration
        var timestamp = _cacheTimestamps[key];
        if ((DateTime.Now - timestamp).TotalSeconds > 1) // ISSUE: 1 second is too short
        {
            _globalCache.Remove(key);
            _cacheTimestamps.Remove(key);
            return null;
        }

        return _globalCache[key];
    }

    // ISSUE: no Remove method, no Clear method, no size limit
    // ISSUE: no generic Get<T> method - requires casting everywhere
    // ISSUE: no IDisposable, no way to clean up
}
