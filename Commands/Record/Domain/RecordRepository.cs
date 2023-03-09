using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bishop.Helper.Database;
using MongoDB.Driver;

namespace Bishop.Commands.Record.Domain;

public class RecordRepository : Repository<RecordEntity>
{
    private const string CollectionName = "records";

    public RecordRepository() : base(CollectionName)
    {
    }

    public async Task<long> CountByUserAndCategory(ulong userId, CounterCategory category)
    {
        return await Collection.CountDocumentsAsync(
            GetFilterByUserAndCategory(userId, category));
    }

    public async Task<Dictionary<ulong, long>> CountByCategoryGroupByUser(CounterCategory category)
    {
        return (await Collection.FindAsync(GetFilterByCategory(category)))
            .ToList()
            .GroupBy(entity => entity.UserId)
            .ToDictionary(group => group.Key,
                group => group.LongCount());
    }

    public async Task<Dictionary<CounterCategory, long>> CountByUserGroupByCategory(ulong userId)
    {
        return (await Collection.FindAsync(GetFilterByUser(userId)))
            .ToList()
            .GroupBy(entity => entity.Category)
            .ToDictionary(group => group.Key,
                group => group.LongCount());
    }

    public async Task<List<RecordEntity>> FindByUserAndCategory(ulong userId, CounterCategory category)
    {
        var cursor = await Collection.FindAsync(
            GetFilterByUserAndCategory(userId, category),
            GetOrderByTimestamp());

        return await cursor.ToListAsync();
    }

    public async Task<List<RecordEntity>> FindByUser(ulong userId)
    {
        var cursor = await Collection.FindAsync(
            GetFilterByUser(userId),
            GetOrderByTimestamp());

        return await cursor.ToListAsync();
    }

    public async Task<List<RecordEntity>> FindByCategory(CounterCategory category)
    {
        var cursor = await Collection.FindAsync(
            GetFilterByCategory(category),
            GetOrderByTimestamp());

        return await cursor.ToListAsync();
    }

    /// <summary>
    ///     Creates and returns a Mongo filter targeting the combination of the provided user IDza and category.
    /// </summary>
    /// <param name="userId">Username to look for.</param>
    /// <param name="counterCategory">Category to look for.</param>
    /// <returns>A Mongo filter.</returns>
    private static FilterDefinition<RecordEntity> GetFilterByUserAndCategory(ulong userId,
        CounterCategory counterCategory)
    {
        return Builders<RecordEntity>
            .Filter.And(
                Builders<RecordEntity>.Filter.Eq("UserId", userId),
                Builders<RecordEntity>.Filter.Eq("Category", counterCategory));
    }

    /// <summary>
    ///     Creates and returns a Mongo filter targeting the provided user id.
    /// </summary>
    /// <param name="userId">Username to look for.</param>
    /// <returns>A Mongo filter.</returns>
    private static FilterDefinition<RecordEntity> GetFilterByUser(ulong userId)
    {
        return Builders<RecordEntity>.Filter.Eq("UserId", userId);
    }

    /// <summary>
    ///     Creates and returns a Mongo filter targeting the provided user id.
    /// </summary>
    /// <param name="category">Category to look for.</param>
    /// <returns>A Mongo filter.</returns>
    private static FilterDefinition<RecordEntity> GetFilterByCategory(CounterCategory category)
    {
        return Builders<RecordEntity>.Filter.Eq("Category", category);
    }


    /// <summary>
    ///     Creates and returns a Mongo sorting option (by timestamp, ascending)
    /// </summary>
    /// <returns>A Mongo sorting option.</returns>
    private static FindOptions<RecordEntity, RecordEntity> GetOrderByTimestamp()
    {
        return new FindOptions<RecordEntity, RecordEntity>
        {
            Sort = Builders<RecordEntity>.Sort.Descending("Timestamp")
        };
    }
}