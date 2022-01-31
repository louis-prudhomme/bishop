using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bishop.Commands.History;
using DSharpPlus.Entities;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Driver;

namespace Bishop.Commands.Meter
{
    /// <summary>
    ///     This class represents a @user's score in a certain category.
    /// </summary>
    [Obsolete("This class was badly designed (no layering + no slicing) and will be removed ; hopefully, sooner than later.")]
    public class Enumerat
    {
        /// <summary>
        ///     Default name of the Mongo collection. Can be overriden by environment variables. <see cref="Program" />.
        /// </summary>
        private const string CollectionName = "meter";

        /// <summary>
        ///     Used to differentiate new records that must be created in database from those that must be updated
        /// </summary>
        private readonly bool _nue;

        private Enumerat(string user, CountCategory key)
        {
            User = user;
            Key = key;
            Score = 0;
            History = new List<Record>();

            _nue = true;
        }

        public static MongoClient Mongo { get; set; }
        public static string Database { get; set; }

        // Properties must have both accessors, as Mongo will need both to get or set values.
        [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        public string Id { get; set; }

        /// <summary>
        ///     Public Get/Set both necessary to deserialize from Mongo
        /// </summary>
        public string User { get; set; }

        public CountCategory Key { get; set; }
        public long Score { get; set; }
        public List<Record> History { get; set; }

        /// <summary>
        ///     Returns the Mongo Collection for the meter.
        /// </summary>
        private static IMongoCollection<Enumerat> Collection =>
            Mongo.GetDatabase(Database).GetCollection<Enumerat>(CollectionName);

        /// <summary>
        ///     Commits the present record against the database.
        /// </summary>
        public async Task Commit()
        {
            if (_nue)
                await Collection.InsertOneAsync(this);
            else
                await Collection.UpdateOneAsync(GetFilter(User, Key),
                    Builders<Enumerat>.Update
                        .Set("Score", Score)
                        .Set("History", History));
        }

        /// <summary>
        ///     Returns all the user records for the provided category.
        /// </summary>
        /// <param name="countCategory">Category to look for.</param>
        /// <returns>List of all matching records.</returns>
        public static async Task<List<Enumerat>> FindAllAsync(CountCategory countCategory)
        {
            return await Collection
                .Find(Builders<Enumerat>.Filter.Eq("Key", countCategory))
                .SortByDescending(enumerat => enumerat.Score)
                .ToListAsync();
        }

        /// <summary>
        ///     Returns all the user records with a history.
        /// </summary>  
        /// <returns>List of all matching records.</returns>
        public static async Task<List<Enumerat>> FindAllWithHistoryAsync()
        {
            return await Collection
                .Find(Builders<Enumerat>.Filter.Exists("History"))
                .SortByDescending(enumerat => enumerat.Score)
                .ToListAsync();
        }

        /// <summary>
        ///     Returns all the user records with a history.
        /// </summary>
        /// <returns>List of all matching records.</returns>
        public static async Task<List<Enumerat>> FindAllWithHistoryAsync(DiscordMember member)
        {
            return await Collection
                .Find(Builders<Enumerat>.Filter.And(Builders<Enumerat>.Filter.Exists("History"),
                    Builders<Enumerat>.Filter.Eq("User", member.Username)))
                .SortByDescending(enumerat => enumerat.Score)
                .ToListAsync();
        }

        /// <summary>
        ///     Fetches the record corresponding to a user and a category
        ///     or creates a new one if the combination does not exist.
        /// </summary>
        /// <param name="user">Pseudo of the user</param>
        /// <param name="meterCategory">Key of the category</param>
        /// <returns>The corresponding record or a new one.</returns>
        public static async Task<List<Enumerat>> FindAllAsync(DiscordUser user)
        {
            return Enum.GetValues(typeof(CountCategory))
                .OfType<CountCategory>()
                .Select(category => (category, Collection.FindAsync(GetFilter(user, category))))
                .Select(tuple => tuple.Item2.Result
                    .FirstOrDefaultAsync()
                    .Result ?? new Enumerat(user.Username, tuple.category))
                .ToList();
        }

        /// <summary>
        ///     Fetches the record corresponding to a user and a category
        ///     or creates a new one if the combination does not exist.
        /// </summary>
        /// <param name="user">Pseudo of the user</param>
        /// <param name="countCategory">Key of the category</param>
        /// <returns>The corresponding record or a new one.</returns>
        public static async Task<Enumerat> FindAsync(DiscordUser user, CountCategory countCategory)
        {
            return await Collection.Find(GetFilter(user, countCategory))
                       .FirstOrDefaultAsync()
                   ?? new Enumerat(user.Username, countCategory);
        }

        /// <summary>
        ///     Creates and returns a Mongo filter targeting the combination of the provided username and category.
        /// </summary>
        /// <param name="user">Username to look for.</param>
        /// <param name="countCategory">Category to look for.</param>
        /// <returns>A Mongo filter.</returns>
        private static FilterDefinition<Enumerat> GetFilter(string user, CountCategory countCategory)
        {
            return Builders<Enumerat>
                .Filter.And(
                    Builders<Enumerat>.Filter.Eq("User", user),
                    Builders<Enumerat>.Filter.Eq("Key", countCategory));
        }

        /// <inheritdoc cref="GetFilter(string,CountCategory)" />
        private static FilterDefinition<Enumerat> GetFilter(DiscordUser user, CountCategory countCategory)
        {
            return GetFilter(user.Username, countCategory);
        }

        public override string ToString()
        {
            return $"{User}’s {Key} ⇒ {Score}";
        }
    }
}