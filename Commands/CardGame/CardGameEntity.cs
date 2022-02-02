using System;
using Bishop.Helper;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

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


    public override string ToString()
    {
        return $"• *{Name}*, offered by **{GifterUserId}** on the {DateHelper.FromDateTimeToString(Date)}";
    }
}