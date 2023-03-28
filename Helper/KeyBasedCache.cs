using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Bishop.Helper.Extensions;

namespace Bishop.Helper;

/// <summary>
///     Interface denoting an asynchronous key-based cache. Its purpose is to
///     hold values and mark the last time they were updated.
///     Identification of values is made through a key which is supposed unique.
///     Value update is left at the discretion of implementations.
///     <seealso cref="AutoUpdatingKeyBasedCache{TKey,TValue}" />
/// </summary>
/// <typeparam name="TKey">Unique key</typeparam>
/// <typeparam name="TValue">Values to cache away</typeparam>
public interface IKeyBasedCache<TKey, TValue> where TKey : notnull
{
    public Task<Cache> Get(TKey key);

    public async Task<TValue?> GetValue(TKey key)
    {
        return (await Get(key)).Value;
    }

    /// <summary>
    ///     Real cache type ; shows the value as well as the last time it was updated.
    /// </summary>
    /// <param name="Value">To save</param>
    /// <param name="FetchAt">Unix timestamp (seconds)</param>
    public record Cache(TValue? Value, long FetchAt);
}

/// <summary>
///     A key-based cache relying on a <see cref="ConcurrentDictionary{TKey,TValue}" />.
///     Allows to update values.
/// </summary>
/// <typeparam name="TKey">Unique identifier.</typeparam>
/// <typeparam name="TValue">Values cached away.</typeparam>
internal class ConcurrentKeyBasedCache<TKey, TValue> : IKeyBasedCache<TKey, TValue> where TKey : notnull
{
    private readonly ConcurrentDictionary<TKey, IKeyBasedCache<TKey, TValue>.Cache> _underlying = new();

    public Task<IKeyBasedCache<TKey, TValue>.Cache> Get(TKey key)
    {
        return Task.FromResult(_underlying.GetOrAdd(key, _ =>
            new IKeyBasedCache<TKey, TValue>.Cache(default, 0)));
    }

    /// <summary>
    ///     Relies on <see cref="ConcurrentDictionaryAdditions.AddOrUpdate{TKey,TValue}" />.
    ///     Regardless of key existence, the provided value will be added to the dictionary.
    ///     Timestamp is computed before insertion.
    /// </summary>
    /// <param name="key">Identifier</param>
    /// <param name="value">Value to cache</param>
    /// <returns></returns>
    public IKeyBasedCache<TKey, TValue>.Cache Set(TKey key, TValue value)
    {
        var cached = new IKeyBasedCache<TKey, TValue>.Cache(value, DateHelper.CurrentEpoch);
        _underlying.AddOrUpdate(key, cached);
        return cached;
    }
}

/// <summary>
///     A readonly <see cref="IKeyBasedCache{TKey,TValue}" /> implementation which also takes a generator to automatically
///     update values when cache is due.
///     No other update is allowed.
/// </summary>
/// <typeparam name="TKey">Identifier</typeparam>
/// <typeparam name="TValue">Values to cache away</typeparam>
public class AutoUpdatingKeyBasedCache<TKey, TValue> : IKeyBasedCache<TKey, TValue> where TKey : notnull
{
    private readonly long _cacheFor;
    private readonly Func<TKey, Task<TValue>> _fetcher;
    private readonly ConcurrentKeyBasedCache<TKey, TValue> _underlying = new();

    public AutoUpdatingKeyBasedCache(long cacheFor, Func<TKey, Task<TValue>> fetcher)
    {
        _cacheFor = cacheFor;
        _fetcher = fetcher;
    }

    /// <summary>
    ///     Returns the most up to date cached value. Will update before returning if:
    ///     - cached value is null for given key (which should not happen)
    ///     - cache is expired
    /// </summary>
    /// <param name="key">Identifier</param>
    /// <returns>Currently cached value.</returns>
    public async Task<IKeyBasedCache<TKey, TValue>.Cache> Get(TKey key)
    {
        var cached = await _underlying.Get(key);
        if (cached.Value != null && DateHelper.CurrentEpoch - cached.FetchAt <= _cacheFor)
            return cached;

        var newValue = _underlying.Set(key, await _fetcher(key));

        return newValue;
    }
}