using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Bishop.Helper.Extensions;

namespace Bishop.Helper;

public interface IKeyBasedCache<TKey, TValue> where TKey : notnull
{
    public record Cache(TValue? Value, long FetchAt);

    public Task<Cache> Get(TKey key);
    public async Task<TValue?> GetValue(TKey key) => (await Get(key)).Value;
}

internal class ConcurrentKeyBasedCache<TKey, TValue>: IKeyBasedCache<TKey, TValue> where TKey : notnull
{
    private readonly ConcurrentDictionary<TKey, IKeyBasedCache<TKey, TValue>.Cache> _underlying = new();

    public Task<IKeyBasedCache<TKey, TValue>.Cache> Get(TKey key)
    {
        return Task.FromResult(_underlying.GetOrAdd(key, _ =>
            new IKeyBasedCache<TKey, TValue>.Cache(default, 0)));
    }

    public IKeyBasedCache<TKey, TValue>.Cache Set(TKey key, TValue value)
    {
        var cached = new IKeyBasedCache<TKey, TValue>.Cache(value, DateHelper.CurrentEpoch);
        _underlying.AddOrUpdate(key, cached);
        return cached;
    }
}

public class AutoUpdatingKeyBasedCache<TKey, TValue> : IKeyBasedCache<TKey, TValue> where TKey : notnull
{
    private readonly ConcurrentKeyBasedCache<TKey, TValue> _underlying = new();
    
    private readonly long _cacheFor;
    private readonly Func<TKey, Task<TValue>> _fetcher;

    public AutoUpdatingKeyBasedCache(long cacheFor, Func<TKey, Task<TValue>> fetcher)
    {
        _cacheFor = cacheFor;
        _fetcher = fetcher;
    }

    public async Task<IKeyBasedCache<TKey, TValue>.Cache> Get(TKey key)
    {
        var cached = await _underlying.Get(key);
        if (cached.Value != null && DateHelper.CurrentEpoch - cached.FetchAt <= _cacheFor)
            return cached;
        
        var newValue = _underlying.Set(key, await _fetcher(key));
        
        return newValue;
    }
}