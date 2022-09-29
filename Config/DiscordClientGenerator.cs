using System;
using Bishop.Commands.CardGame;
using Bishop.Commands.Dump;
using Bishop.Commands.History;
using Bishop.Commands.Weather;
using Bishop.Config.Converters;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Converters;
using Microsoft.Extensions.DependencyInjection;

namespace Bishop.Config;

/// <summary>
///     Generates the configuration of the Discord Client.
/// </summary>
public class DiscordClientGenerator
{
    /// <summary>
    ///     Can be overriden by environment variables. See <see cref="Program" />.
    /// </summary>
    private static readonly string BaseSigil = Environment
        .GetEnvironmentVariable("COMMAND_SIGIL") ?? ";";

    private static readonly string DiscordToken = Environment
        .GetEnvironmentVariable("DISCORD_TOKEN")!;

    private readonly CommandsNextExtension _commands;

    private readonly string[] _sigil;

    public DiscordClientGenerator()
    {
        _sigil = new[] {BaseSigil};
        Client = new DiscordClient(AssembleConfig());

        _commands = Client.UseCommandsNext(AssembleCommands(AssembleServiceCollection()));
        _commands.SetHelpFormatter<DefaultHelpFormatter>();

        _commands.RegisterConverter(new MeterKeysConverter());
        _commands.RegisterConverter(new WeatherMetricConverter());
    }

    public DiscordClient Client { get; }
    public string Sigil => string.Join(" ", _sigil);

    private IServiceCollection AssembleServiceCollection()
    {
        var nestedCache = new UserNameCache();
        var nestedScoreFormatter = new ScoreFormatter
        {
            Cache = nestedCache
        };
        var nestedRecordsService = new RecordService
        {
            Cache = nestedCache,
            Random = new Random(),
            Repository = new RecordRepository()
        };
        var nestedCounterService = new CounterService
        {
            RecordRepository = new RecordRepository(),
            ScoreFormatter = nestedScoreFormatter,
            HistoryService = nestedRecordsService
        };
        var nestedUserNameCacheService = new UserNameCacheService
        {
            Cache = nestedCache
        };
        var nestedWeatherService = new WeatherService
        {
            Accessor = new WeatherAccessor()
        };

        return new ServiceCollection()
            .AddSingleton(nestedRecordsService)
            .AddSingleton(nestedCounterService)
            .AddSingleton(nestedCache)
            .AddSingleton(nestedUserNameCacheService)
            .AddSingleton(nestedWeatherService)
            .AddSingleton(nestedScoreFormatter)
            .AddSingleton<RecordRepository>()
            .AddSingleton<CardGameRepository>()
            .AddSingleton<HoroscopeRepository>();
    }

    private CommandsNextConfiguration AssembleCommands(IServiceCollection services)
    {
        return new CommandsNextConfiguration
        {
            Services = services.BuildServiceProvider(),
            StringPrefixes = _sigil
        };
    }

    private DiscordConfiguration AssembleConfig()
    {
        return new DiscordConfiguration
        {
            Token = DiscordToken,
            TokenType = TokenType.Bot,
            Intents = DiscordIntents.AllUnprivileged | DiscordIntents.GuildMembers
        };
    }

    public void Register<T>() where T : BaseCommandModule
    {
        _commands.RegisterCommands<T>();
    }

    public void RegisterBulk(params Type[] types)
    {
        foreach (var type in types)
        {
            _commands.RegisterCommands(type);
        }
    }
}