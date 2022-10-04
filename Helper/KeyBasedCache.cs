using System;
using System.Collections.Concurrent;

namespace Bishop.Helper;

public interface IKeyBasedCache<TKey, TValue> where TKey : notnull
{
    public record Cache(TValue? Value, long FetchAt);
    public Cache Get(TKey key);
}

public class ConcurrentKeyBasedCache<TKey, TValue>: IKeyBasedCache<TKey, TValue> where TKey : notnull
{
    private readonly ConcurrentDictionary<TKey, IKeyBasedCache<TKey, TValue>.Cache> _underlying = new();

    public IKeyBasedCache<TKey, TValue>.Cache Get(TKey key)
    {
        return _underlying.GetOrAdd(key, _ => new IKeyBasedCache<TKey, TValue>.Cache(default, 0));
    }

    public IKeyBasedCache<TKey, TValue>.Cache Set(TKey key, TValue value)
    {
        var cached = new IKeyBasedCache<TKey, TValue>.Cache(value, DateHelper.CurrentEpoch);
        _underlying.AddOrUpdate(key,
            k => cached,
            (_, cache) => cache);
        return cached;
    }
}

public class AutoUpdatingKeyBasedCache<TKey, TValue> : IKeyBasedCache<TKey, TValue> where TKey : notnull
{
    private readonly ConcurrentKeyBasedCache<TKey, TValue> _underlying = new();
    
    private readonly long _cacheFor;
    private readonly Func<TKey,TValue> _fetcher;

    public AutoUpdatingKeyBasedCache(long cacheFor, Func<TKey, TValue> fetcher)
    {
        _cacheFor = cacheFor;
        _fetcher = fetcher;
        
    }

    public IKeyBasedCache<TKey, TValue>.Cache Get(TKey key)
    {
        var cached = _underlying.Get(key);
        if (DateHelper.CurrentEpoch - cached.FetchAt <= _cacheFor)
            return cached;
        
        var newValue = _underlying.Set(key, _fetcher(key));
        
        return newValue;
    }
}