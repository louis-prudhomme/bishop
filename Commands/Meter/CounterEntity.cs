using System;
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

    public CounterEntity(ulong user, CountCategory key) 
    {
        UserId = user;
        Key = key;
        Score = 0;
    }

    /// <summary>
    ///     To convert oldish and deprecated <see cref="Enumerat" /> to <see cref="CounterEntity" />.
    /// </summary>
    /// <param name="userId">Discord user ID</param>
    /// <param name="old">Previous <see cref="Enumerat" /></param>
    [Obsolete("Will eventually be removed along with Enumerats.")]
    public CounterEntity(ulong userId, Enumerat old) 
    {
        UserId = userId;
        Key = old.Key;
        Score = old.Score;
    }

    /// <summary>
    ///     Public Get/Set both necessary to deserialize from Mongo
    /// </summary>
    public ulong UserId { get; set; }

    public CountCategory Key { get; set; }
    public long Score { get; set; }

    //TODO create a formatter.
    public override string ToString()
    {
        return $"{UserId}’s {Key} ⇒ {Score}";
    }
}