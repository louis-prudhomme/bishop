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

    /// <summary>
    ///     Creates and returns a Mongo filter targeting the combination of the provided username and category.
    /// </summary>
    /// <param name="user">Username to look for.</param>
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

    private FindOptions<RecordEntity, RecordEntity> GetOrderByTimestamp()
    {
        return new FindOptions<RecordEntity, RecordEntity>()
        {
            Sort = Builders<RecordEntity>.Sort.Ascending("Timestamp")
        };
    } 
}