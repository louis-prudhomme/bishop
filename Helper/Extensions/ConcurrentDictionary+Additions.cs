using System;
using System.Collections.Concurrent;

namespace Bishop.Helper.Extensions;

public static class ConcurrentDictionaryAdditions
{
    public static TValue AddOrUpdate<TKey, TValue>(this ConcurrentDictionary<TKey, TValue> self, TKey key, TValue value) where TKey : notnull
    {
        if (self == null)
            throw new ArgumentNullException(nameof(self));
        self.AddOrUpdate(key,
            _ => value,
            (_, _) => value);
        return value;
    }
}