using System;
using System.Reflection;
using System.Threading.Tasks;
using Bishop.Commands.CardGame;
using Bishop.Commands.Dump;
using Bishop.Commands.History;
using Bishop.Commands.Meter;
using Bishop.Commands.Meter.Aliases;
using Bishop.Commands.Weather;
using Bishop.Config;
using Bishop.Helper;
using DSharpPlus;
using log4net;
using log4net.Config;

namespace Bishop;

internal class Program
{
<<<<<<< HEAD
    private const string TomatoFilePath = "./Resources/tomatoes.json";
    private const string AledFilePath = "./Resources/aleds.json";
    private const string QuoteFilePath = "./Resources/quotes.json";
    private const string StalkFilePath = "./Resources/slenders.json";

    private static readonly string DiscordToken = Environment
        .GetEnvironmentVariable("DISCORD_TOKEN")!;

    private static readonly string MongoToken = Environment
        .GetEnvironmentVariable("MONGO_TOKEN")!;

    private static readonly string MongoDatabase = Environment
        .GetEnvironmentVariable("MONGO_DB")!;

    private static readonly string CommandSigil = Environment
        .GetEnvironmentVariable("COMMAND_SIGIL")!;

||||||| b64cc2a
    private const string TomatoFilePath = "./Resources/tomatoes.json";
    private const string StalkFilePath = "./Resources/slenders.json";

    private static readonly string DiscordToken = Environment
        .GetEnvironmentVariable("DISCORD_TOKEN")!;

    private static readonly string MongoToken = Environment
        .GetEnvironmentVariable("MONGO_TOKEN")!;

    private static readonly string MongoDatabase = Environment
        .GetEnvironmentVariable("MONGO_DB")!;

    private static readonly string CommandSigil = Environment
        .GetEnvironmentVariable("COMMAND_SIGIL")!;

=======
>>>>>>> origin/fucking-weather
    private static readonly ILog Log = LogManager
        .GetLogger(MethodBase.GetCurrentMethod()?
            .DeclaringType);

    private static DiscordClient _discord = null!;
    private static DiscordClientGenerator _generator = null!;

    [STAThread]
    private static void Main()
    {
        XmlConfigurator.Configure();

<<<<<<< HEAD
        Tomato.Tomatoes = new TomatoConfigurator(TomatoFilePath)
            .ReadTomatoesAsync()
            .Result;

        Aled.Aleds = new AledConfigurator(AledFilePath)
            .ReadAledsAsync()
            .Result;
        try
        {
            Quote.Quotes = new QuoteConfigurator(QuoteFilePath)
                .ReadQuotesAsync()
                .Result;
        }
        catch (Exception e)
        {
            Log.Error(e);
        }

        Stalk.Lines = new StalkConfigurator(StalkFilePath)
            .ReadStalkAsync()
            .Result;

        var mongoClient = new MongoClient(MongoToken);

        var dbContext = new MongoContext(mongoClient, MongoDatabase);
        Repository<CardGameEntity>.MongoContext = dbContext;
        Repository<CounterEntity>.MongoContext = dbContext;
        Repository<RecordEntity>.MongoContext = dbContext;

        _generator = new DiscordClientGenerator(DiscordToken, CommandSigil, dbContext);
||||||| b64cc2a
        Tomato.Tomatoes = new TomatoConfigurator(TomatoFilePath)
            .ReadTomatoesAsync()
            .Result;

        Stalk.Lines = new StalkConfigurator(StalkFilePath)
            .ReadStalkAsync()
            .Result;

        var mongoClient = new MongoClient(MongoToken);

        var dbContext = new MongoContext(mongoClient, MongoDatabase);
        Repository<CardGameEntity>.MongoContext = dbContext;
        Repository<CounterEntity>.MongoContext = dbContext;
        Repository<RecordEntity>.MongoContext = dbContext;

        _generator = new DiscordClientGenerator(DiscordToken, CommandSigil, dbContext);
=======
        _generator = new DiscordClientGenerator();
>>>>>>> origin/fucking-weather

        _generator.Register<Randomizer>();
        _generator.Register<Stalk>();
        _generator.Register<Tomato>();
        _generator.Register<Aled>();
        _generator.Register<Quote>();
        _generator.Register<Vote>();
        _generator.Register<Deleter>();

        _generator.Register<BdmCounter>();
        _generator.Register<BeaufCounter>();
        _generator.Register<SauceCounter>();
        _generator.Register<SelCounter>();
        _generator.Register<RassCounter>();

        _generator.Register<CounterService>();
        _generator.Register<RecordService>();
        _generator.Register<CardGameService>();

        _generator.Register<WeatherController>();

        _generator.Register<UserNameCacheService>();

        _discord = _generator.Client;
        AdaptUserIdTo.UserNameAsync = async id => (await _discord.GetUserAsync(id)).Username;
        GuildFetcher.FetchAsync = async id => await _discord.GetGuildAsync(id);

        Log.Info($"Sigil is {_generator.Sigil}");
        Log.Info("Awaiting commands");

        MainAsync()
            .GetAwaiter()
            .GetResult();
    }

    private static async Task MainAsync()
    {
        await _discord.ConnectAsync();
        await Task.Delay(-1);
    }
}