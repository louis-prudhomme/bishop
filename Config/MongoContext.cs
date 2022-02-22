using System;
using MongoDB.Driver;

namespace Bishop.Config;

public class MongoContext
{
    private static readonly string MongoToken = Environment
        .GetEnvironmentVariable("MONGO_TOKEN")!;

    public static MongoClient Mongo { get; } = new(MongoToken);

    public static string Database { get; } = Environment
        .GetEnvironmentVariable("MONGO_DB")!;
}