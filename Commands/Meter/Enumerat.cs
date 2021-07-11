using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bishop.Commands.Meter
{
    partial class Enumerat
    {
        private const string CollectionName = "meter";
        public static MongoClient Mongo;
        public static string Database;

        [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        public string Id { get; set; }
        public string User { get; set; }
        public Keys Key { get; set; }
        public int Score { get; set; }

        private readonly bool _nue = false;

        private Enumerat(string user, Keys key)
        {
            User = user;
            Key = key;
            Score = 0;
            _nue = true;
        }

        public async Task Commit()
        {
            try
            {
                if (isNew)
                    await Collection.InsertOneAsync(this);
                else await Collection.UpdateOneAsync(GetFilter(User, Key),
                    Builders<Enumerat>.Update.Set("Score", Score));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
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

        private bool isNew => _nue;
        private static IMongoCollection<Enumerat> Collection => Mongo.GetDatabase(Database).GetCollection<Enumerat>(CollectionName);
    }
}
