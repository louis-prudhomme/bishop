using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Driver;

namespace Bishop.Commands.Meter
{
    internal class Enumerat
    {
        private const string COLLECTION_NAME = "meter";
        public static MongoClient Mongo;
        public static string Database;

        private readonly bool _nue;

        private Enumerat(string user, Keys key)
        {
            User = user;
            Key = key;
            Score = 0;

            _nue = true;
        }

        [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        public string Id { get; set; }

        public string User { get; set; }
        public Keys Key { get; set; }
        public long Score { get; set; }

        private static IMongoCollection<Enumerat> Collection =>
            Mongo.GetDatabase(Database).GetCollection<Enumerat>(COLLECTION_NAME);

        public async Task Commit()
        {
            if (_nue)
                await Collection.InsertOneAsync(this);
            else
                await Collection.UpdateOneAsync(GetFilter(User, Key),
                    Builders<Enumerat>.Update.Set("Score", Score));
        }

        public static async Task<List<Enumerat>> FindAllAsync(Keys key)
        {
            return await Collection
                .Find(Builders<Enumerat>.Filter.Eq("Key", key))
                .ToListAsync();
        }

        public static async Task<Enumerat> FindAsync(string user, Keys key)
        {
            return await Collection.Find(GetFilter(user, key))
                       .FirstOrDefaultAsync()
                   ?? new Enumerat(user, key);
        }

        private static FilterDefinition<Enumerat> GetFilter(string user, Keys key)
        {
            return Builders<Enumerat>
                .Filter.And(
                    Builders<Enumerat>.Filter.Eq("User", user),
                    Builders<Enumerat>.Filter.Eq("Key", key));
        }

        public override string ToString()
        {
            return $"{User}’s {Key} ⇒ {Score}";
        }
    }
}