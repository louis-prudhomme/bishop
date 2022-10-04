using System;
using System.Collections.Generic;

namespace Bishop.Helper.Extensions;

public static class Enumerable
{
    public static string Join(this IEnumerable<string> source)
    {
        if (source == null)
            throw new ArgumentNullException(nameof(source));
        using var enumerator = source.GetEnumerator();

        if (!enumerator.MoveNext())
            throw new InvalidOperationException("no elements");

        var acc = enumerator.Current;
        while (enumerator.MoveNext())
            acc = string.Join(acc, enumerator.Current);
        return acc;
    }
    
    public static string JoinWithNewlines(this IEnumerable<string> source)
    {
        if (source == null)
            throw new ArgumentNullException(nameof(source));
        using var enumerator = source.GetEnumerator();

        if (!enumerator.MoveNext())
            throw new InvalidOperationException("no elements");

        var acc = enumerator.Current;
        while (enumerator.MoveNext())
            acc = string.Join("\n", acc, enumerator.Current);
        return acc;
    }
}