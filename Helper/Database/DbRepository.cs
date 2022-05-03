using System.Collections.Generic;
using System.Threading.Tasks;
using Bishop.Config;
using MongoDB.Driver;

namespace Bishop.Helper.Database;

public abstract class Repository<T> where T : DbEntity
{
    /// <summary>
    ///     Default name of the Mongo collection. Should be a constant in children classes.
    ///     This will be the name of the set in database containing data. Think of it as a table.
    /// </summary>
    private readonly string _collectionName;

    protected Repository(string collectionName)
    {
        _collectionName = collectionName;
    }

    /// <summary>
    ///     Returns the Mongo Collection for the meter.
    /// </summary>
    protected IMongoCollection<T> Collection =>
        MongoContext.Mongo.GetDatabase(MongoContext.Database).GetCollection<T>(_collectionName);

    /// <summary>
    ///     Allows to insert several instances asynchronously.
    /// </summary>
    /// <param name="instances">To insert</param>
    public async Task InsertManyAsync(IEnumerable<T> instances)
    {
        await Collection.InsertManyAsync(instances);
    }

    /// <summary>
    ///     Saves an instance regardless of its status (newly created or modified) asynchronously.
    /// </summary>
    /// <param name="instance">To save.</param>
    public async Task SaveAsync(T instance)
    {
        if (instance.IsNew)
            await Collection.InsertOneAsync(instance);
        else
            await Collection.ReplaceOneAsync(GetIdFilter(instance.Id!), instance);
    }

    /// <summary>
    ///     Through the specified filter, tries to return the first corresponding instance or null.
    /// </summary>
    /// <param name="filter">Filter to select instance with.</param>
    /// <returns>Nullable value, depending on result existence.</returns>
    public async Task<T?> FindOne(FilterDefinition<T> filter)
    {
        var cursor = await Collection.FindAsync(filter);
        return await cursor.FirstAsync();
    }

    /// <summary>
    ///     Through the specified filter, selects any matching instance.
    /// </summary>
    /// <param name="filter">Filter to select instances with.</param>
    /// <returns>Empty collection if none match.</returns>
    public async Task<List<T>> FindAllAsync(FilterDefinition<T> filter)
    {
        var cursor = await Collection.FindAsync(filter);
        return await cursor.ToListAsync();
    }

    /// <summary>
    ///     Returns a list of every record. Use wisely, as this can be performance-heavy.
    /// </summary>
    /// <returns>Empty collection when there are no records.</returns>
    public async Task<List<T>> FindAllAsync()
    {
        return await FindAllAsync(FilterDefinition<T>.Empty);
    }

    /// <summary>
    ///     Returns a filter for an entity ID.
    /// </summary>
    /// <param name="id">To select.</param>
    /// <returns>A filter for the specified ID.</returns>
    protected FilterDefinition<T> GetIdFilter(string id)
    {
        return Builders<T>.Filter.Eq("Id", id);
    }
}