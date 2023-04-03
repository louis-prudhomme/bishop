using System.Collections.Generic;

namespace Bishop.Helper.Extensions;

public static class DictionaryAdditions
{
    public static TValue? TryGet<TKey, TValue>(this Dictionary<TKey, TValue> self, TKey key) where TKey : notnull
    {
        self.TryGetValue(key, out var value);
        return value;
    }
}