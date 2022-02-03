using System.Collections.Generic;
using System.Linq;
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

    public async Task<CounterEntity?> FindOneByUserAndCategory(ulong userId, CounterCategory category)
    {
        var cursor = await Collection.FindAsync(
            GetFilterByUserAndCategory(userId, category));
        var scores = await cursor.ToListAsync();

        return scores.Any() ? scores.First() : null;
    }

    public async Task<List<CounterEntity>> FindByUser(ulong userId)
    {
        var cursor = await Collection.FindAsync(
            GetFilterByUser(userId));

        return await cursor.ToListAsync();
    }

    public async Task<List<CounterEntity>> FindByCategory(CounterCategory category)
    {
        var cursor = await Collection.FindAsync(
            GetFilterByCategory(category), GetOrderByScore());

        return await cursor.ToListAsync();
    }

    /// <summary>
    ///     Creates and returns a Mongo filter targeting the combination of the provided user IDza and category.
    /// </summary>
    /// <param name="userId">Username to look for.</param>
    /// <param name="counterCategory">Category to look for.</param>
    /// <returns>A Mongo filter.</returns>
    private FilterDefinition<CounterEntity> GetFilterByUserAndCategory(ulong userId, CounterCategory counterCategory)
    {
        return Builders<CounterEntity>
            .Filter.And(
                Builders<CounterEntity>.Filter.Eq("UserId", userId),
                Builders<CounterEntity>.Filter.Eq("Category", counterCategory));
    }

    /// <summary>
    ///     Creates and returns a Mongo filter targeting the provided category.
    /// </summary>
    /// <param name="counterCategory">Category to look for.</param>
    /// <returns>A Mongo filter.</returns>
    private FilterDefinition<CounterEntity> GetFilterByCategory(CounterCategory counterCategory)
    {
        return Builders<CounterEntity>.Filter.Eq("Category", counterCategory);
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
        return new FindOptions<CounterEntity, CounterEntity>
        {
            Sort = Builders<CounterEntity>.Sort.Descending("Score")
        };
    }
}