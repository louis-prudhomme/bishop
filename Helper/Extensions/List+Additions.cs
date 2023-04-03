using System;
using System.Collections;
using System.Collections.Generic;

namespace Bishop.Helper.Extensions;

public static class ListAdditions
{
    private static readonly Random Rand = new();

    public static T? Random<T>(this IList<T> self)
    {
        if (self == null)
            throw new ArgumentNullException(nameof(self));

        return self.Count == 0 ? default : self[Rand.Next(0, self.Count)];
    }

    public static bool IsEmpty(this IList self)
    {
        return self.Count == 0;
    }
}