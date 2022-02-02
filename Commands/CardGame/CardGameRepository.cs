using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bishop.Helper;
using MongoDB.Driver;

namespace Bishop.Commands.CardGame;

/// <summary>
///     Specifies and implements interactions of <see cref="CardGameEntity" /> with DB.
/// </summary>
public class CardGameRepository : Repository<CardGameEntity>
{
    private const string CollectionName = "decks";

    public CardGameRepository() : base(CollectionName)
    {
    }
}