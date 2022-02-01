using System.Collections.Generic;
using System.Threading.Tasks;
using Bishop.Helper;
using MongoDB.Driver;

namespace Bishop.Commands.Meter;

/// <summary>
///     Specifies and implements interactions of <see cref="CounterEntity" /> with DB.
/// </summary>
public class CounterRepository : Repository<CounterEntity>
{
    private const string CollectionName = "counters";

    public CounterRepository() : base(CollectionName)
    {
    }

    public async Task<CounterEntity?> FindByUserAndCategory(ulong userId, CountCategory category)
    {
        var cursor = await Collection.FindAsync(
            GetFilterByUserAndCategory(userId, category));

        if (await cursor.AnyAsync())
            return await cursor.FirstOrDefaultAsync();
        return null;
    }

    /// <summary>
    ///     Creates and returns a Mongo filter targeting the combination of the provided user IDza and category.
    /// </summary>
    /// <param name="userId">Username to look for.</param>
    /// <param name="countCategory">Category to look for.</param>
    /// <returns>A Mongo filter.</returns>
    private FilterDefinition<CounterEntity> GetFilterByUserAndCategory(ulong userId, CountCategory countCategory)
    {
        return Builders<CounterEntity>
                .Filter.And(
                    Builders<CounterEntity>.Filter.Eq("UserId", userId),
                    Builders<CounterEntity>.Filter.Eq("Category", countCategory));
    }

    /// <summary>
    ///     Creates and returns a Mongo filter targeting the provided user id.
    /// </summary>
    /// <param name="userId">Username to look for.</param>
    /// <returns>A Mongo filter.</returns>
    private FilterDefinition<CounterEntity> GetFilterByUser(ulong userId)
    {
        return Builders<CounterEntity>.Filter.Eq("UserId", userId);
    }

    private FindOptions<CounterEntity, CounterEntity> GetOrderByScore()
    {
        return new FindOptions<CounterEntity, CounterEntity>()
        {
            Sort = Builders<CounterEntity>.Sort.Descending("Score")
        };
    }
}