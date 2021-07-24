﻿using System.Collections.Generic;
using System.Threading.Tasks;
using DSharpPlus.Entities;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Driver;

namespace Bishop.Commands.Meter
{
    /// <summary>
    /// This class represents a @user's score in a certain category. 
    /// </summary>
    internal class Enumerat
    {
        /// <summary>
        /// Default name of the Mongo collection. Can be overriden by environment variables. <see cref="Program"/>.
        /// </summary>
        private const string COLLECTION_NAME = "meter";
        public static MongoClient Mongo { get; set; }
        public static string Database { get; set; }

        /// <summary>
        /// Used to differentiate new records that must be created in database from those that must be updated 
        /// </summary>
        private readonly bool _nue;

        private Enumerat(string user, MeterCategories meterCategory)
        {
            User = user;
            MeterCategory = meterCategory;
            Score = 0;

            _nue = true;
        }

        // Properties must have both accessors, as Mongo will need both to get or set values.
        [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        public string Id { get; set; }
        public string User { get; set; }
        public MeterCategories MeterCategory { get; set; }
        public long Score { get; set; }

        /// <summary>
        /// Returns the Mongo Collection for the meter.
        /// </summary>
        private static IMongoCollection<Enumerat> Collection =>
            Mongo.GetDatabase(Database).GetCollection<Enumerat>(COLLECTION_NAME);

        /// <summary>
        /// Commits the present record against the database.
        /// </summary>
        public async Task Commit()
        {
            if (_nue)
                await Collection.InsertOneAsync(this);
            else 
                await Collection.UpdateOneAsync(GetFilter(User, MeterCategory),
                    Builders<Enumerat>.Update.Set("Score", Score));
        }

        /// <summary>
        /// Returns all the user records for the provided category.
        /// </summary>
        /// <param name="meterCategory">Category to look for.</param>
        /// <returns>List of all matching records.</returns>
        public static async Task<List<Enumerat>> FindAllAsync(MeterCategories meterCategory)
        {
            return await Collection
                .Find(Builders<Enumerat>.Filter.Eq("Key", meterCategory))
                .ToListAsync();
        }

        /// <summary>
        /// Fetches the record corresponding to a user and a category
        /// or creates a new one if the combination does not exist.
        /// </summary>
        /// <param name="user">Pseudo of the user</param>
        /// <param name="meterCategory">Key of the category</param>
        /// <returns>The corresponding record or a new one.</returns>
        public static async Task<Enumerat> FindAsync(DiscordUser user, MeterCategories meterCategory)
        {
            return await Collection.Find(GetFilter(user, meterCategory))
                       .FirstOrDefaultAsync()
                   ?? new Enumerat(user.Username, meterCategory);
        }
        
        /// <summary>
        /// Creates and returns a Mongo filter targeting the combination of the provided username and category.
        /// </summary>
        /// <param name="user">Username to look for.</param>
        /// <param name="meterCategory">Category to look for.</param>
        /// <returns>A Mongo filter.</returns>
        private static FilterDefinition<Enumerat> GetFilter(string user, MeterCategories meterCategory)
        {
            return Builders<Enumerat>
                .Filter.And(
                    Builders<Enumerat>.Filter.Eq("User", user),
                    Builders<Enumerat>.Filter.Eq("Key", meterCategory));
        }
        
        /// <inheritdoc cref="GetFilter(string,Bishop.Commands.Meter.MeterCategories)"/>
        private static FilterDefinition<Enumerat> GetFilter(DiscordUser user, MeterCategories meterCategory)
        {
            return GetFilter(user.Username, meterCategory);
        }

        public override string ToString()
        {
            return $"{User}’s {MeterCategory} ⇒ {Score}";
        }
    }
}