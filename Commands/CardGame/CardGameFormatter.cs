using System.Threading.Tasks;
using Bishop.Helper;

namespace Bishop.Commands.CardGame;

public class CardGameFormatter
{
    public IKeyBasedCache<ulong, string> Cache { private get; set; } = null!;
    
    public async Task<string> Format(CardGameEntity cardGame)
    {
        return $"• *{cardGame.Name}*, " +
               $"offered by **{await Cache.GetValue(cardGame.GifterUserId)}** " +
               $"the {DateHelper.FromDateTimeToStringDate(cardGame.Date)}";
    }
}