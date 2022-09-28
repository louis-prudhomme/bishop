using System;
using System.Threading.Tasks;
using Bishop.Helper.Database;

namespace Bishop.Commands.Meter;

/// <summary>
///     Represents users' counters of points in a specific category.
/// </summary>
public class CounterEntity : DbEntity
{
    public CounterEntity() { }

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

}