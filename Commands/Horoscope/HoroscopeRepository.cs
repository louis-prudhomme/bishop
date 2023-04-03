using Bishop.Helper.Database;

namespace Bishop.Commands.Horoscope;

/// <summary>
///     Specifies and implements interactions of <see cref="HoroscopeEntity" /> with DB.
/// </summary>
public class HoroscopeRepository : Repository<HoroscopeEntity>
{
    private const string CollectionName = "horoscopes";

    public HoroscopeRepository() : base(CollectionName)
    {
    }
}