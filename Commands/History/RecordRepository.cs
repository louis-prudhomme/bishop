using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bishop.Commands.Meter;
using Bishop.Helper;
using MongoDB.Driver;

namespace Bishop.Commands.History;

public class RecordRepository : Repository<RecordEntity>
{
    private const string CollectionName = "records";

    public RecordRepository() : base(CollectionName)
    {
    }

    public async Task<List<RecordEntity>> FindByUserAndCategory(ulong userId, CountCategory category)
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

    /// <summary>
    ///     Creates and returns a Mongo filter targeting the combination of the provided user IDza and category.
    /// </summary>
    /// <param name="userId">Username to look for.</param>
    /// <param name="countCategory">Category to look for.</param>
    /// <returns>A Mongo filter.</returns>
    private FilterDefinition<RecordEntity> GetFilterByUserAndCategory(ulong userId, CountCategory countCategory)
    {
        return Builders<RecordEntity>
                .Filter.And(
                    Builders<RecordEntity>.Filter.Eq("UserId", userId),
                    Builders<RecordEntity>.Filter.Eq("Key", countCategory))
            ;
    }

    /// <summary>
    ///     Creates and returns a Mongo filter targeting the provided user id.
    /// </summary>
    /// <param name="userId">Username to look for.</param>
    /// <returns>A Mongo filter.</returns>
    private FilterDefinition<RecordEntity> GetFilterByUser(ulong userId)
    {
        return Builders<RecordEntity>.Filter.Eq("UserId", userId);
    }

    private FindOptions<RecordEntity, RecordEntity> GetOrderByTimestamp()
    {
        return new FindOptions<RecordEntity, RecordEntity>()
        {
            Sort = Builders<RecordEntity>.Sort.Ascending("Timestamp")
        };
    } 
}