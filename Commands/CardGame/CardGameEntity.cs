﻿using System;
using System.Threading.Tasks;
using Bishop.Helper;

namespace Bishop.Commands.CardGame;

public class CardGameEntity : DbObject
{
    public CardGameEntity(string name, ulong gifterUserId)
    {
        Name = name;
        Date = DateTime.Now;
        Timestamp = DateHelper.FromDateTimeToTimestamp(Date);
        GifterUserId = gifterUserId;
    }

    public CardGameEntity(CardGame old, ulong gifterUserId)
    {
        Name = old.Name;
        Date = DateHelper.FromOldStringToDateTime(old.Date);
        Timestamp = DateHelper.FromDateTimeToTimestamp(Date);
        GifterUserId = gifterUserId;
    }

    public string Name { get; set; }
    public ulong GifterUserId { get; set; }
    public DateTime Date { get; set; }
    public long Timestamp { get; set; }

    [Obsolete("use ToString(mapper) instead.")]
    public override string ToString()
    {
        return $"• *{Name}*, offered by **{GifterUserId}** the {DateHelper.FromDateTimeToString(Date)}";
    }

    public string ToString(Func<ulong, string> idToNameMapper)
    {
        return $"• *{Name}*, offered by **{idToNameMapper(GifterUserId)}** the {DateHelper.FromDateTimeToString(Date)}";
    }

    public async Task<string> ToString(Func<ulong, Task<string>> idToNameMapper)
    {
        return $"• *{Name}*, offered by **{await idToNameMapper(GifterUserId)}** the {DateHelper.FromDateTimeToString(Date)}";
    }
}