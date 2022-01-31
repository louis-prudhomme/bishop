using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace Bishop.Commands.CardGame;

/// <summary>
///     This class represents a @user's score in a certain category.
/// </summary>
internal class CardCollection
{
    /// <summary>
    ///     Default name of the Mongo collection. Can be overriden by environment variables. <see cref="Program" />.
    /// </summary>
    private const string CollectionName = "cards";

    public static MongoClient Mongo { get; set; }
    public static string Database { get; set; }

    /// <summary>
    ///     Returns the Mongo Collection for the meter.
    /// </summary>
    private static IMongoCollection<CardGame> Collection =>
        Mongo.GetDatabase(Database).GetCollection<CardGame>(CollectionName);

    public static async Task AddAsync(CardGame item)
    {
        await Collection.InsertOneAsync(item);
    }

    public static async Task<List<CardGame>> FindAllAsync()
    {
        return await Collection.FindAsync(FilterDefinition<CardGame>.Empty).Result.ToListAsync();
    }
}