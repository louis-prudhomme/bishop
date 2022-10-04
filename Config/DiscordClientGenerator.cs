using System;
using System.Collections.Immutable;
using Bishop.Commands.CardGame;
using Bishop.Commands.Dump;
using Bishop.Commands.Record.Domain;
using Bishop.Commands.Record.Presenter;
using Bishop.Commands.Weather.Domain;
using Bishop.Commands.Weather.Service;
using Bishop.Config.Converters;
using Bishop.Helper;
using Bishop.Helper.Grive;
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
        // CACHES
        var nestedGriveCache =
            new AutoUpdatingKeyBasedCache<GriveDirectory, ImmutableList<string>>(GriveWalker.CacheFor,
                GriveWalker.FetchFiles);
        var nestedWeatherCache =
            new AutoUpdatingKeyBasedCache<string, WeatherEntity>(WeatherAccessor.CacheForSeconds,
                WeatherAccessor.CurrentSync);
        var nestedUserNameCacheService =
            new AutoUpdatingKeyBasedCache<ulong, string>(UserNameAccessor.CacheForSeconds,
                UserNameAccessor.FetchUserNameSync);
        // OTHERS
        var nestedScoreFormatter = new ScoreFormatter
        {
            Cache = nestedUserNameCacheService
        };
        var nestedRecordsService = new RecordController
        {
            Cache = nestedUserNameCacheService,
            Random = new Random(),
            Repository = new RecordRepository(),
            ScoreFormatter = nestedScoreFormatter,
        };
        var nestedWeatherService = new WeatherService
        {
            Cache = nestedWeatherCache
        };

        return new ServiceCollection()
            .AddSingleton(nestedRecordsService)
            .AddSingleton<IKeyBasedCache<GriveDirectory, ImmutableList<string>>>(nestedGriveCache)
            .AddSingleton<IKeyBasedCache<string, WeatherEntity>>(nestedWeatherCache)
            .AddSingleton<IKeyBasedCache<ulong, string>>(nestedUserNameCacheService)
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