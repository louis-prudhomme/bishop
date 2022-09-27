using System;
using Bishop.Commands.CardGame;
using Bishop.Commands.Dump;
using Bishop.Commands.History;
using Bishop.Commands.Meter;
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
        var nestedRecordsService = new RecordService
        {
            Cache = nestedCache,
            Random = new Random(),
            Repository = new RecordRepository()
        };
        var nestedCounterService = new CounterService
        {
            CounterRepository = new CounterRepository(),
            Cache = nestedCache,
            HistoryService = nestedRecordsService
        };
        var nestedUserNameCacheService = new UserNameCacheService
        {
            Cache = nestedCache
        };
        var weatherService = new WeatherService
        {
            Accessor = new WeatherAccessor()
        };
        //var credentialsService = new GriveCredentialsService();
        var grive = new Grive
        {
            Service = null!
        };

        return new ServiceCollection()
            .AddSingleton(nestedRecordsService)
            .AddSingleton(nestedCounterService)
            .AddSingleton(nestedCache)
            .AddSingleton(nestedUserNameCacheService)
            .AddSingleton(weatherService)
            .AddSingleton(grive)
            .AddSingleton<RecordRepository>()
            .AddSingleton<CounterRepository>()
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