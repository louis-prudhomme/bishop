using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bishop.Helper.Extensions;

public static class EnumerableAdditions
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
    
    public static IEnumerable<int> CumulativeSum(this IEnumerable<int> sequence)
    {
        var sum = 0;
        foreach(var item in sequence)
        {
            sum += item;
            yield return sum;
        }        
    }
    
    public static async Task<TResult> WhenAll<TSource, TResult>(this IEnumerable<Task<TSource>> self, Func<IEnumerable<TSource>, TResult> action)
    {
        if (self == null)
            throw new ArgumentNullException(nameof(self));
        var selves = await Task.WhenAll(self);
        return action(selves);
    }
}