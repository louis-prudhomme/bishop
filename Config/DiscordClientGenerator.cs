using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using Bishop.Commands.CardGame;
using Bishop.Commands.Horoscope;
using Bishop.Commands.Record.Business;
using Bishop.Commands.Record.Domain;
using Bishop.Commands.Record.Controller;
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
        UserNameAccessor.FetchUserName = async id => (await Client.GetUserAsync(id)).Username;
        // TODO find a better way to add conditions per-folder rather than globally
        var griveWalker = new GriveWalker(new List<FileCheck>
        {
            Path.HasExtension,
            path => GriveWalker.AuthorizedExtensions.Contains(Path.GetExtension(path).ToLowerInvariant()),
            path => new FileInfo(path).Length < GriveWalker.DiscordFileSizeLimitBytes
        });
        // CACHES
        var nestedGriveCache =
            new AutoUpdatingKeyBasedCache<GriveDirectory, ImmutableList<string>>(GriveWalker.CacheForSeconds,
                griveWalker.FetchFilesAsync);
        var nestedWeatherCache =
            new AutoUpdatingKeyBasedCache<string, WeatherEntity>(WeatherAccessor.CacheForSeconds,
                WeatherAccessor.Current);
        var nestedUserNameCacheService =
            new AutoUpdatingKeyBasedCache<ulong, string>(UserNameAccessor.CacheForSeconds,
                UserNameAccessor.FetchUserName);
        // OTHERS
        var nestedRecordRepository = new RecordRepository();
        var nestedRecordManager = new RecordManager
        {
            Repository = nestedRecordRepository
        };
        var nestedPlotManager = new PlotManager();
        var nestedRecordsController = new RecordController
        {
            Cache = nestedUserNameCacheService,
            Random = new Random(),
            Formatter = new RecordFormatter(),
            Manager = nestedRecordManager,
            PlotManager = nestedPlotManager
        };
        var nestedWeatherService = new WeatherService
        {
            Cache = nestedWeatherCache
        };
        var nestedCardGameFormatter = new CardGameFormatter
        {
            Cache = nestedUserNameCacheService
        };

        return new ServiceCollection()
            .AddSingleton(nestedRecordsController)
            .AddSingleton(nestedWeatherService)
            .AddSingleton<IKeyBasedCache<GriveDirectory, ImmutableList<string>>>(nestedGriveCache)
            .AddSingleton<IKeyBasedCache<string, WeatherEntity>>(nestedWeatherCache)
            .AddSingleton<IKeyBasedCache<ulong, string>>(nestedUserNameCacheService)
            .AddSingleton(nestedPlotManager)
            .AddSingleton(nestedRecordManager)
            .AddSingleton(nestedCardGameFormatter)
            .AddSingleton<RecordFormatter>()
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

    private static DiscordConfiguration AssembleConfig()
    {
        return new DiscordConfiguration
        {
            Token = DiscordToken,
            TokenType = TokenType.Bot,
            Intents = DiscordIntents.AllUnprivileged | DiscordIntents.GuildMembers | DiscordIntents.MessageContents
        };
    }

    public void RegisterBulk(params Type[] types)
    {
        foreach (var type in types)
        {
            _commands.RegisterCommands(type);
        }
    }
}