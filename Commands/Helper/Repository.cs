using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Bishop.Config;
using MongoDB.Driver;

namespace Bishop.Commands.Helper
{
    public abstract class Repository<T> where T : DbObject
    {
        /// <summary>
        ///     Default name of the Mongo collection. Can be overriden by environment variables. <see cref="Program" />.
        /// </summary>
        private readonly string _collectionName;

        public static MongoContext MongoContext { private get; set; }

        protected Repository(string collectionName)
        {
            _collectionName = collectionName;
        }
        
        
        /// <summary>
        ///     Returns the Mongo Collection for the meter.
        /// </summary>
        protected IMongoCollection<T> Collection =>
            MongoContext.Mongo.GetDatabase(MongoContext.Database).GetCollection<T>(_collectionName);

        public async Task InsertManyAsync(IEnumerable<T> instances)
        {
            await Collection.InsertManyAsync(instances);
        }
        
        public async Task SaveAsync(T instance)
        {
            if (instance.IsNew)
                await Collection.InsertOneAsync(instance);
            else
                await Collection.ReplaceOneAsync(GetIdFilter(instance.Id), instance);
        }

        public async Task<T> FindOne(FilterDefinition<T> filter)
        {
            var cursor = await Collection.FindAsync(filter);
            return await cursor.FirstAsync();
        }
        
        public async Task<List<T>> FindAllAsync(FilterDefinition<T> filter)
        {
            var cursor = await Collection.FindAsync(filter);
            return await cursor.ToListAsync();
        }

        public async Task<List<T>> FindAllAsync()
        {
            return await FindAllAsync(FilterDefinition<T>.Empty);
        }

        public FilterDefinition<T> GetIdFilter(string id)
        {
            return Builders<T>.Filter.Eq("Id", id);
        }
    }
}