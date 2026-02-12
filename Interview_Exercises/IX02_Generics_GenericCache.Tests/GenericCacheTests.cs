using IX02_Generics_GenericCache;
using Xunit;
using FluentAssertions;

namespace IX02_Generics_GenericCache.Tests;

/// <summary>
/// TODO: Write unit tests for GenericCache.
/// Use FakeTimeProvider to control time in your tests.
///
/// Tests to write:
/// 1. Adding and retrieving a value before expiry returns true and correct value.
/// 2. TryGet returns false after the item has expired.
/// 3. Adding a duplicate key throws ArgumentException.
/// 4. Remove returns true for existing key, false for missing key.
/// 5. Count does not include expired entries.
/// 6. Cleanup removes expired entries, allowing keys to be reused with Add.
/// 7. Cache works with different type parameters (e.g., int keys, object values).
/// </summary>
public class GenericCacheTests
{
    private readonly FakeTimeProvider _fakeTime; 
    private readonly GenericCache<string, int> _cache;

    public GenericCacheTests()
    {
        _fakeTime = new FakeTimeProvider();
        _cache = new GenericCache<string, int>(_fakeTime);
    }

    [Fact]
    public void Add_ShouldAddItemToCache()
    {
        _cache.Add("key", 1, TimeSpan.FromMinutes(2));
        _fakeTime.Advance(TimeSpan.FromMinutes(1));
        _cache.TryGet("key", out var value).Should().BeTrue();
        value.Should().Be(1);

        _cache.Count.Should().Be(1);
    }

    [Fact]
    public void TryGet_ShouldReturnFalseAfterExpiry()
    {
        _cache.Add("key", 1, TimeSpan.FromMinutes(1));
        _fakeTime.Advance(TimeSpan.FromMinutes(2));
        _cache.TryGet("key", out var value).Should().BeFalse();
        value.Should().Be(0);
    }

    [Fact]
    public void Add_ShouldThrowArgumentExceptionIfKeyAlreadyExists()
    {
        _cache.Add("key", 1, TimeSpan.FromMinutes(1));
        Action action = () => _cache.Add("key", 2, TimeSpan.FromMinutes(1));
        action.Should().Throw<ArgumentException>();
        _cache.Count.Should().Be(1);
        _cache.TryGet("key", out var value).Should().BeTrue();
        value.Should().Be(1);
    }

    [Fact]
    public void Remove_ShouldReturnTrueAfterRemovingKey()
    {
        _cache.Add("key", 1, TimeSpan.FromMinutes(1));
        _cache.Remove("key").Should().BeTrue();
        _cache.Count.Should().Be(0);
        _cache.TryGet("key", out var value).Should().BeFalse();
    }

    [Fact]
    public void Remove_ShouldReturnFalseAfterMissingKey()
    {
        _cache.Add("key", 1, TimeSpan.FromMinutes(1));
        _cache.Remove("key2").Should().BeFalse();
        _cache.Count.Should().Be(1);
        _cache.TryGet("key", out var value).Should().BeTrue();
    }

    [Fact]
    public void Count_ShouldNotIncludeExpiredEntries()
    {
        _cache.Add("key", 0, TimeSpan.FromMinutes(5));
        _cache.Add("key1", 1, TimeSpan.FromMinutes(2));
        _cache.Add("key2", 2, TimeSpan.FromMinutes(1));
        _fakeTime.Advance(TimeSpan.FromMinutes(2));
        _cache.Count.Should().Be(1);
    }

    [Fact]
    public void Cleanup_ShouldRemoveExpiredEntriesAllowingKeyReutilization()
    {
        _cache.Add("key", 0, TimeSpan.FromMinutes(5));
        _cache.Add("key1", 1, TimeSpan.FromMinutes(1));
        _cache.Add("key2", 2, TimeSpan.FromMinutes(1));

        _fakeTime.Advance(TimeSpan.FromMinutes(2));

        _cache.Cleanup();

        _cache.Add("key1", 3, TimeSpan.FromMinutes(5));
        _cache.Add("key2", 4, TimeSpan.FromMinutes(5));

        _fakeTime.Advance(TimeSpan.FromMinutes(2));

        _cache.TryGet("key1", out var value1).Should().BeTrue();
        value1.Should().Be(3);

        _cache.TryGet("key2", out var value2).Should().BeTrue();
        value2.Should().Be(4);
        
        _cache.Count.Should().Be(3);
    }

    [Fact]
    public void Add_ShouldWorkWithDifferentParameterTypes()
    {
        _cache.Add("int_value", 42, TimeSpan.FromMinutes(5));
        _cache.TryGet("int_value", out var intVal).Should().BeTrue();
        intVal.Should().Be(42);

        var objectCache = new GenericCache<int, object>(_fakeTime);
        objectCache.Add(1, "hello", TimeSpan.FromMinutes(5));
        objectCache.TryGet(1, out var objVal).Should().BeTrue();
        objVal.Should().Be("hello");
    }




}
