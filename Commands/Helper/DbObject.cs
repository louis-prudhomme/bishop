using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace Bishop.Commands.Helper
{
    public abstract class DbObject
    {
        // Properties must have both accessors, as Mongo will need both to get or set values.
        [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        public string Id { get; set; }

        /// <summary>
        ///     Used to differentiate new records that must be created in database from those that must be updated
        /// </summary>
        private readonly bool _nue;


        protected DbObject(bool nue)
        {
            _nue = nue;
        }

        public bool IsNew => _nue;
    }
}