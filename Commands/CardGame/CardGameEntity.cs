using System;
using Bishop.Helper;
using Bishop.Helper.Database;

namespace Bishop.Commands.CardGame;

public class CardGameEntity : DbEntity
{
    public CardGameEntity(string name, ulong gifterUserId)
    {
        Name = name;
        Date = DateTime.Now;
        Timestamp = DateHelper.FromDateTimeToTimestamp(Date);
        GifterUserId = gifterUserId;
    }

    public string Name { get; set; }
    public ulong GifterUserId { get; set; }
    public DateTime Date { get; set; }
    public long Timestamp { get; set; }

    public async Task<string> ToString(Func<ulong, Task<string>> idToNameMapper)
    {
        return $"• *{Name}*, " +
               $"offered by **{await idToNameMapper(GifterUserId)}** " +
               $"the {DateHelper.FromDateTimeToStringDate(Date)}";
    }
}