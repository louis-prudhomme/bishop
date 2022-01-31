using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace Bishop.Helper
{
    /// <summary>
    ///     Class representing a generic DB entity. It comes with an ID and making its staus explicit.
    /// </summary>
    public abstract class DbObject
    {
        protected DbObject(bool nue)
        {
            IsNew = nue;
        }

        // Properties must have both accessors, as Mongo will need both to get or set values.
        [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        public string Id { get; set; }

        /// <summary>
        ///     Used to differentiate new records that must be created in database from those that must be updated
        ///     TODO remove this and use ID instead
        /// </summary>
        public bool IsNew { get; }
    }
}