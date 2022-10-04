using System;
using System.Threading.Tasks;
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

    public string ToString(Func<ulong, string> idToNameMapper)
    {
        return
            $"• *{Name}*, offered by **{idToNameMapper(GifterUserId)}** the {DateHelper.FromDateTimeToStringDate(Date)}";
    }
}