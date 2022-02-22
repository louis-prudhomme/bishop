using System;
using Google.Apis.Auth.OAuth2.Requests;
using MongoDB.Driver;

namespace Bishop.Config;

public class MongoContext
{
    private static readonly string MongoToken = Environment
        .GetEnvironmentVariable("MONGO_TOKEN")!;
    private static readonly string MongoDatabase = Environment
        .GetEnvironmentVariable("MONGO_DB")!;
    
    private static readonly MongoClient MongoClient = new (MongoToken);

    public static MongoClient Mongo => MongoClient;
    public static string Database => MongoDatabase;
}