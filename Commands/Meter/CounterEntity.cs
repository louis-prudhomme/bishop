using System;
using System.Threading.Tasks;
using Bishop.Helper;

namespace Bishop.Commands.Meter;

/// <summary>
///     Represents users' counters of points in a specific category.
/// </summary>
public class CounterEntity : DbObject
{
    public CounterEntity()
    {
    }

    public CounterEntity(ulong user, CounterCategory category)
    {
        UserId = user;
        Category = category;
        Score = 0;
    }

    /// <summary>
    ///     Public Get/Set both necessary to deserialize from Mongo
    /// </summary>
    public ulong UserId { get; set; }

    public CounterCategory Category { get; set; }
    public long Score { get; set; }

    public async Task<string> ToString(Func<ulong, Task<string>> idToNameMapper)
    {
        return $"{await idToNameMapper(UserId)}’s {Category} ⇒ {Score}";
    }
}