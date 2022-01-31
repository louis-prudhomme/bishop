using MongoDB.Driver;

namespace Bishop.Config
{
    public class MongoContext
    {
        public MongoContext(MongoClient mongo, string database)
        {
            Mongo = mongo;
            Database = database;
        }

        public MongoClient Mongo { get; }
        public string Database { get; }
    }
}